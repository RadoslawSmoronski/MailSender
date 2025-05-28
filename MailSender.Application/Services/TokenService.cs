using MailSender.Application.Interfaces;
using MailSender.Domain.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MailSender.Application.Services
{
    /// <summary>
    /// Service responsible for generating JWT access tokens for client applications.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Creates a signed JWT access token based on the provided client data and signing key.
        /// </summary>
        /// <param name="clientAppDto">DTO containing client application identifier.</param>
        /// <param name="signingKey">The secret key used to sign the JWT token.</param>
        /// <returns>A JWT token string.</returns>
        public async Task<string> CreateAccessTokenAsync(ClientAppDto clientAppDto, string signingKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

            var claims = new[]
            {
                new Claim("client_id", clientAppDto.ClientId),
                new Claim(ClaimTypes.Role, "ClientApp")
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
