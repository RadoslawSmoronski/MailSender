using MailSender.Domain.DTOs;

namespace MailSender.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(SimpleClientAppDto clientApp, string signingKey);
    }
}
