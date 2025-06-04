using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IMailManager
    {
        Task<Result<SendedMailDto>> SendAsync(string? AppId, string? AppName, MailDto mailDto);
    }
}
