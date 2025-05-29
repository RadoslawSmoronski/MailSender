
using MailSender.Common.Result;
using MailSender.Domain.DTOs;
using MailSender.Domain.Settings;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IMailManager
    {
        Result<SendedMailDto> Send(string? AppId, string? AppName, MailDto mailDto, SmtpSettings smtpSettings);
    }
}
