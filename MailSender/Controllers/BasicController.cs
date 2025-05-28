using MailSender.Model.DTOs;
using MailSender.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MailSender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public BasicController(ITokenService tokenService)
        {
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

            var jwt = await _tokenService.CreateAccessTokenAsync(request);

            return Ok(new { token = jwt });
        }
    }
}
