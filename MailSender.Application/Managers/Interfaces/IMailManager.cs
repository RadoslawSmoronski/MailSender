using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Models;
using MailSender.Contracts.Settings;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IMailManager
    {
        Task<Result<SendedMailDto>> SendAsync(string? appId, string? appName, MailDto mailDto);
        Task<Result<List<MailLog>>> GetLogsAsync(string? appId);
    }
}
