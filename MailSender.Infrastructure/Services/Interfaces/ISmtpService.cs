using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;

namespace MailSender.Application.Services.Interfaces
{
    public interface ISmtpService
    {
        Result Send(MailDto mailDto);
    }
}
