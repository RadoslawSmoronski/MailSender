using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using MailSender.Infrastructure.EmailSender.Interfaces;
using Microsoft.Extensions.Options;
using Resend;

namespace MailSender.Infrastructure.EmailSender
{
    public class ResendMailSender : IMailSenderProvider
    {
        private readonly ResendSettings _resendSettings;
        private readonly IResend _resend;

        public ResendMailSender(IResend resend,
            IOptions<ResendSettings> resendSettings)
        {
            _resendSettings = resendSettings.Value;
            _resend = resend;
        }

        public async Task<Result> SendAsync(MailDto mailDto)
        {
            try
            {
                var message = new EmailMessage();
                message.From = _resendSettings.SenderEmail;
                message.To.Add(mailDto.To);
                message.Subject = mailDto.Subject;
                message.HtmlBody = mailDto.Body;

                var result = await _resend.EmailSendAsync(message);

                if(result.Success)
                {
                    return Result.Success();
                }

                return Error.Unknown("Unknown error");
            }
            catch (Exception ex)
            {
                return Error.Unknown(ex.Message);
            }
        }

    }
}

//ToRefactor
