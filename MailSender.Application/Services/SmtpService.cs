using MailKit.Security;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailSender.Domain.DTOs;

namespace MailSender.Application.Services
{
    public class SmtpService : ISmtpService
    {
        public Result Send(MailDto mailDto, SmtpDto smtpDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpDto.User, smtpDto.User));
            message.To.Add(new MailboxAddress(mailDto.To, mailDto.To));
            message.Subject = mailDto.Subject;

            message.Body = new TextPart("html")
            {
                Text = mailDto.Body
            };

            using var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
            {
                return true;
            };

            client.Connect(smtpDto.Host, smtpDto.Port, SecureSocketOptions.StartTls);

            client.Authenticate(smtpDto.User, smtpDto.Password);

            client.Send(message);
            client.Disconnect(true);

            return Result.Success();
        }
    }
}
