
using MailSender.Domain.DTOs;

namespace MailSender.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(ClientAppDto clientAppDto, string signingKey);
    }
}
