using MailSender.Contracts.DTOs;

namespace MailSender.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(SimpleClientAppDto clientApp, string signingKey);
    }
}
