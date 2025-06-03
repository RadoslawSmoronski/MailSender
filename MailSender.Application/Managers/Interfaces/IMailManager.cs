using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IMailManager
    {
        Result<SendedMailDto> Send(string? AppId, string? AppName, MailDto mailDto, SmtpSettings smtpSettings);
    }
}
