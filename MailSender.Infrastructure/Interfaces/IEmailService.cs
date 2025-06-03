using MailSender.Common.Result;

namespace MailSender.Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Result Send(MailRequest mailRequest);
    }
}
