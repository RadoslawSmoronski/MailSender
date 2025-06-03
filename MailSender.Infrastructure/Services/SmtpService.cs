using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit.Security;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailSender.Application.Services
{
    public class SmtpService : ISmtpService
    {
        private readonly SmtpSettings _smtpSettings;

        public SmtpService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }


        public Result Send(MailDto mailDto)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.User, _smtpSettings.User));
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

            client.Connect(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);

            client.Authenticate(_smtpSettings.User, _smtpSettings.Password);

            client.Send(message);
            client.Disconnect(true);

            return Result.Success();
        }
    }
}
