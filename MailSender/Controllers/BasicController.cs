using MailSender.Application.Interfaces;
using MailSender.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public BasicController(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        private static readonly List<ClientAppDto> Clients = new()
        {
            new ClientAppDto { ClientId = "app1", ClientSecret = "secret1" },
            new ClientAppDto { ClientId = "app2", ClientSecret = "secret2" }
        };

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] ClientAppDto request)
        {
            var client = Clients.SingleOrDefault( c => c.ClientId == request.ClientId && c.ClientSecret == request.ClientSecret);

            if (client == null)
                return Unauthorized();

            var signingJwtKey = _configuration["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey", "Signing key must be provided in configuration.");

            var jwt = await _tokenService.CreateAccessTokenAsync(request, signingJwtKey);

            return Ok(new { token = jwt });
        }
    }
}
