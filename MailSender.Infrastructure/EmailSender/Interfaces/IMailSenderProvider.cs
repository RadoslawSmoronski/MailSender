using MailSender.Common.Result;
using MailSender.Contracts.DTOs;

namespace MailSender.Infrastructure.EmailSender.Interfaces
{
    public interface IMailSenderProvider
    {
        Task<Result> SendAsync(MailDto mailDto);
    }
}
