using MailKit.Net.Smtp;
using MailKit.Security;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using MailSender.Infrastructure.EmailSender.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Net;

namespace MailSender.Infrastructure.EmailSender
{
    /// <summary>
    /// Implementation of <see cref="IMailSenderProvider"/> for sending emails via Brevo API and SMTP.
    /// </summary>
    public class BrevoMailSender : IMailSenderProvider
    {
        private readonly BrevoSettings _brevoSettings;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrevoMailSender"/> class.
        /// Sets up HTTP client headers for API key authentication.
        /// </summary>
        /// <param name="brevoSettings">Configuration settings for Brevo SMTP and API.</param>
        /// <param name="httpClient">Injected HTTP client instance for making requests.</param>
        public BrevoMailSender(IOptions<BrevoSettings> brevoSettings, HttpClient httpClient)
        {
            _brevoSettings = brevoSettings.Value;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add("api-key", _brevoSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Sends an email asynchronously using Brevo's REST API.
        /// </summary>
        /// <param name="mailDto">The email data transfer object containing recipient, subject, and body.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the send operation.</returns>
        public async Task<Result> SendAsync(MailDto mailDto)
        {
            try
            {
                var emailData = new
                {
                    sender = new { email = _brevoSettings.SenderEmail, name = _brevoSettings.User },
                    to = new[] { new { email = mailDto.To } },
                    subject = mailDto.Subject,
                    htmlContent = mailDto.Body
                };

                var json = JsonSerializer.Serialize(emailData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", content);

                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Error.Unauthorized("You have not been authenticated. Make sure the provided api-key is correct");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return Error.Failure("Unexpected error: " + errorContent);
            }
            catch (Exception ex)
            {
                return Error.Failure("Unexpected error: " + ex.Message);
            }
        }

        /// <summary>
        /// Sends an email asynchronously using SMTP protocol via Brevo SMTP server.
        /// </summary>
        /// <param name="mailDto">The email data transfer object containing recipient, subject, and body.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the send operation.</returns>
        public async Task<Result> SendSmtpAsync(MailDto mailDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_brevoSettings.SenderEmail, _brevoSettings.SenderEmail));
            message.To.Add(new MailboxAddress(mailDto.To, mailDto.To));
            message.Subject = mailDto.Subject;

            message.Body = new TextPart("html")
            {
                Text = mailDto.Body
            };

            try
            {
                using var client = new SmtpClient();

                // NOTE: Disables certificate validation. Not recommended for production environments.
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                client.Connect(_brevoSettings.Host, _brevoSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_brevoSettings.User, _brevoSettings.Password);
                client.Send(message);
                client.Disconnect(true);

                return Result.Success();
            }
            catch (AuthenticationException ex)
            {
                return Error.Unauthentication("Authentication failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                return Error.Failure("Unexpected error: " + ex.Message);
            }
        }
    }
}
