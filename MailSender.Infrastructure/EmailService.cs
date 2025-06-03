using MailKit.Security;
using MailSender.Common.Result;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MailSender.Infrastructure.Interfaces;
using MailSender.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace MailSender.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public Result Send(MailRequest mailRequest)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpSettings.User, _smtpSettings.User));
            message.To.Add(new MailboxAddress(mailRequest.RecipientEmail, mailRequest.RecipientEmail));
            message.Subject = mailRequest.Subject;

            message.Body = new TextPart("html")
            {
                Text = mailRequest.BodyContent
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
