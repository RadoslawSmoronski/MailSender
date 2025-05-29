
using MailSender.Common.Result;
using MailSender.Domain.DTOs;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IMailManager
    {
        Result<SendedMailDto> Send(string? AppId, string? AppName, MailDto mailDto, SmtpDto smtpDto);
    }
}
