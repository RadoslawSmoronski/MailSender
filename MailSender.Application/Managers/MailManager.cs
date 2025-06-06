using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Models;
using MailSender.Infrastructure.EmailSender.Interfaces;

namespace MailSender.Application.Managers
{
    /// <summary>
    /// Manager responsible for sending email messages via SMTP service
    /// and logging failures to the database.
    /// </summary>
    public class MailManager : IMailManager
    {
        private readonly IMailSenderProvider _mailSender;
        private readonly IMailLogService _logService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailManager"/> class with the specified SMTP service and logging service.
        /// </summary>
        /// <param name="mailSender">The SMTP service instance used for sending emails.</param>
        /// <param name="logService">The service used for logging email sending results, especially failures.</param>
        public MailManager(IMailSenderProvider mailSender, IMailLogService logService)
        {
            _mailSender = mailSender;
            _logService = logService;
        }

        /// <summary>
        /// Sends an email message asynchronously and returns the result of the operation.
        /// On failure, logs the error details to the database.
        /// </summary>
        /// <param name="appId">The application identifier sending the email. Cannot be null or empty.</param>
        /// <param name="appName">The name of the application sending the email. Cannot be null or empty.</param>
        /// <param name="mailDto">The DTO object containing the email message data.</param>
        /// <returns>
        /// A <see cref="Result{SendedMailDto}"/> containing the send status and email details on success;
        /// or an <see cref="Error"/> describing the failure, including logging failure if applicable.
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

            var mailLog = new MailLog()
            {
                AppId = appId,
                Recipient = mailDto.To,
                Subject = mailDto.Subject,
                Status = "Failed",
                ErrorMessage = result.Error.Description
            };

            var logResult = await _logService.LogAsync(mailLog);

            if (!logResult.IsSuccess)
            {
                return Error.Unknown($"Failed to save log: {logResult.Error.Description} | Original error: {result.Error.Description}");
            }

            return Error.Unknown(result.Error.Description);
        }
    }
}
