using MailSender.Common.Result;
using MailSender.Contracts.Models;

namespace MailSender.Application.Services.Interfaces
{
    public interface IMailLogService
    {
        Task<Result> LogAsync(MailLog mailLog);
        Task<Result<List<MailLog>>> GetLogs(string AppId);
    }
}
