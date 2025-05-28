using MailSender.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace MailSender.Api.Controllers
{
    [Route("api/client-app")]
    [ApiController]
    public class ClientAppController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            return Ok(registerDto);
        }
    }

}
