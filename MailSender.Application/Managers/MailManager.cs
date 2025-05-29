using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Domain.DTOs;

namespace MailSender.Application.Managers
{
    public class MailManager : IMailManager
    {

        public Result<SendedMailDto> Send(string? AppId, string? AppName, MailDto mailDto)
        {
            //var result = _smptService.Send(mailDto);

            //if (result.IsSuccess)
            //{
            //    return new SendedMailDto
            //    {
            //        AppId = AppId,
            //        AppName = AppName,
            //        Status = "test",
            //        email = mailDto
            //    };
            //}

            return Error.Failure("test");
        }
    }
}
