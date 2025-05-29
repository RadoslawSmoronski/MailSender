using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Domain.DTOs;
using MailSender.Domain.Settings;

namespace MailSender.Application.Managers
{
    public class MailManager : IMailManager
    {
        private readonly ISmtpService _smtpService;
        public MailManager(ISmtpService smtpService)
        {
            _smtpService = smtpService;
        }

        public Result<SendedMailDto> Send(string? AppId, string? AppName, MailDto mailDto, SmtpSettings smtpSettings)
        {
            var result = _smtpService.Send(mailDto, smtpSettings);

            if (result.IsSuccess)
            {
                return new SendedMailDto
                {
                    AppId = AppId,
                    AppName = AppName,
                    Status = "test",
                    email = mailDto
                };
            }

            return Error.Failure("test");
        }
    }
}
