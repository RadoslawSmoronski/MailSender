using MailSender.Common.Result;
using MailSender.Domain.DTOs;

namespace MailSender.Application.Managers.Interfaces
{
    public interface IAuthManager
    {
        Task<Result<RegisteredDto>> RegisterApplicationAsync(RegisterAppDto registerDto);
    }
}
