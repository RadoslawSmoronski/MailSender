using MailSender.Model.DTOs;

namespace MailSender.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(ClientAppDto clientAppDto);
    }
}
