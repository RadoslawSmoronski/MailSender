using MailSender.Model.DTOs;
using MailSender.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MailSender.Services
{
    /// <summary>
    /// Service responsible for generating JSON Web Tokens (JWT) for client applications.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// Retrieves the signing key from configuration.
        /// </summary>
        /// <param name="configuration">Application configuration (used to access JWT settings).</param>
        /// <exception cref="ArgumentNullException">Thrown if signing key is missing in configuration.</exception>
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            var signingKey = _configuration["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey", "Signing key must be provided in configuration.");

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }

        /// <summary>
        /// Creates a signed JWT access token for the given client application.
        /// </summary>
        /// <param name="clientAppDto">DTO containing client application information (e.g. client ID).</param>
        /// <returns>JWT token as a string.</returns>
        public async Task<string> CreateAccessTokenAsync(ClientAppDto clientAppDto)
        {
            var claims = new[]
                        {
                new Claim("client_id", clientAppDto.ClientId),
                new Claim(ClaimTypes.Role, "ClientApp")
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
