using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Infrastructure.EmailSender.Interfaces;

namespace MailSender.Application.Managers
{
    /// <summary>
    /// Manager responsible for sending email messages via SMTP service.
    /// </summary>
    public class MailManager : IMailManager
    {
        private readonly IMailSenderProvider _mailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailManager"/> class with the specified SMTP service.
        /// </summary>
        /// <param name="mailSender">The SMTP service instance used for sending emails.</param>
        public MailManager(IMailSenderProvider mailSender)
        {
            _mailSender = mailSender;
        }

        /// <summary>
        /// Sends an email message and returns the result of the operation along with additional information.
        /// </summary>
        /// <param name="appId">The application identifier sending the email. Cannot be null or empty.</param>
        /// <param name="appName">The name of the application sending the email. Cannot be null or empty.</param>
        /// <param name="mailDto">The DTO object containing the email message data.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing <see cref="SendedMailDto"/> on success;
        /// otherwise, returns an <see cref="Error"/> describing the failure.
        /// </returns>
        public async Task<Result<SendedMailDto>> SendAsync(string? appId, string? appName, MailDto mailDto)
        {
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(appName))
            {
                return Error.Validation("AppId or AppName cannot be empty or null.");
            }

            var result = await _mailSender.SendAsync(mailDto);

            if (result.IsSuccess)
            {
                return new SendedMailDto
                {
                    AppId = appId,
                    AppName = appName,
                    Status = "queued",
                    Email = mailDto
                };
            }
            else if (result.Error != null)
            {
                if (result.Error.errorType == ErrorType.Unauthentication)
                {
                    return Error.Unauthentication(result.Error.Description);
                }
                else
                {
                    return result.Error;
                }
            }

            return Error.Unknown("Unknown problem");
        }
    }
}
