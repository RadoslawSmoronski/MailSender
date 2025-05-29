using MailSender.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Api.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendAsync([FromBody] MailDto sendMailDto)
        {
            return Ok(new SendedMailDto());
        }
    }
}
