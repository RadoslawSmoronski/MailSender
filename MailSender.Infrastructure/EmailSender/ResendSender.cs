using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using MailSender.Infrastructure.EmailSender.Interfaces;
using Microsoft.Extensions.Options;
using Resend;

namespace MailSender.Infrastructure.EmailSender
{
    /// <summary>
    /// Implementation of <see cref="IMailSenderProvider"/> using the Resend email delivery service.
    /// </summary>
    public class ResendMailSender : IMailSenderProvider
    {
        private readonly ResendSettings _resendSettings;
        private readonly IResend _resend;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResendMailSender"/> class.
        /// </summary>
        /// <param name="resend">Injected Resend email service client.</param>
        /// <param name="resendSettings">Configuration settings for the Resend service.</param>
        public ResendMailSender(IResend resend, IOptions<ResendSettings> resendSettings)
        {
            _resendSettings = resendSettings.Value;
            _resend = resend;
        }

        /// <summary>
        /// Sends an email asynchronously using the Resend service.
        /// </summary>
        /// <param name="mailDto">The email data transfer object containing recipient, subject, and body.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure of the send operation.</returns>
        public async Task<Result> SendAsync(MailDto mailDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mailDto.To))
                {
                    return Error.Failure("Recipient email address cannot be empty.");
                }

                var message = new EmailMessage
                {
                    From = _resendSettings.SenderEmail,
                    Subject = mailDto.Subject,
                    HtmlBody = mailDto.Body
                };

                message.To.Add(mailDto.To);

                var result = await _resend.EmailSendAsync(message);

                if (result.Success)
                {
                    return Result.Success();
                }

                return Error.Failure("Unknown error occurred while sending email.");
            }
            catch (Exception ex)
            {
                return Error.Unknown("Unexpected error: " + ex.Message);
            }
        }
    }
}
