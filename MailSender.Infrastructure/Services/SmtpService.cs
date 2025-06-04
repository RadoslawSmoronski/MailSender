using MailKit.Net.Smtp;
using MailKit.Security;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailSender.Application.Services
{
    /// <summary>
    /// Provides functionality to send emails using SMTP via MailKit.
    /// </summary>
    public class SmtpService : ISmtpService
    {
        private readonly SmtpSettings _smtpSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpService"/> class with SMTP settings.
        /// </summary>
        /// <param name="smtpSettings">Injected options containing SMTP configuration.</param>
        public SmtpService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        /// <summary>
        /// Sends an email using SMTP based on the provided <see cref="MailDto"/>.
        /// </summary>
        /// <param name="mailDto">An object containing email recipient, subject, and body content.</param>
        /// <returns>
        /// A <see cref="Result"/> indicating the outcome of the operation.
        /// Returns <c>Success</c> if the email was sent successfully; otherwise returns a failure with the corresponding error message.
        /// </returns>
        /// <exception cref="MailKit.Security.AuthenticationException">
        /// Thrown when authentication with the SMTP server fails.
        /// </exception>
        /// <exception cref="System.Exception">
        /// Thrown for all other unexpected errors.
        /// </exception>
        public Result Send(MailDto mailDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.User, _smtpSettings.User));
            message.To.Add(new MailboxAddress(mailDto.To, mailDto.To));
            message.Subject = mailDto.Subject;

            message.Body = new TextPart("html")
            {
                Text = mailDto.Body
            };

            try
            {
                using var client = new SmtpClient();

                // NOTE: Disables certificate validation. Not recommended for production.
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

                client.Connect(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                client.Authenticate(_smtpSettings.User, _smtpSettings.Password);
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
