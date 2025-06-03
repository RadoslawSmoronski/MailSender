using MailSender.Application.Services.Interfaces;
using MailSender.Contracts.DTOs;
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

        private static readonly List<RegisteredDto> Clients = new()
        {
            new RegisteredDto { AppId = "app1", AppName = "secret1" },
            new RegisteredDto { AppId = "app2", AppName = "secret2" }
        };

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] SimpleClientAppDto request)
        {
            var client = Clients.SingleOrDefault( c => c.AppId == request.AppId && c.AppName == request.AppName);

            if (client == null)
                return Unauthorized();

            var signingJwtKey = _configuration["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey", "Signing key must be provided in configuration.");

            var jwt = _tokenService.CreateAccessToken(request, signingJwtKey);

            return Ok(new { token = jwt });
        }
    }
}
