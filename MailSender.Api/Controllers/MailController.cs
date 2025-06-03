using MailSender.Application.Managers.Interfaces;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MailSender.Api.Controllers
{
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailManager _mailManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SmtpSettings _settings;

        public MailController(IMailManager mailManager, IHttpContextAccessor httpContextAccessor, IOptions<SmtpSettings> options)
        {
            _mailManager = mailManager;
            _httpContextAccessor = httpContextAccessor;
            _settings = options.Value;
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendAsync([FromBody] MailDto sendMailDto)
        {
            var claimsPrincipal = HttpContext.User;

            var appId = claimsPrincipal.FindFirst("app_id")?.Value;
            var appName = claimsPrincipal.FindFirst("app_name")?.Value;

            var result = _mailManager.Send(appId, appName, sendMailDto, _settings);

            if(result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Problem(
                statusCode: 500,
                title: "InternalServerError",
                detail: "An unexpected error occurred."
            );
        }
    }
}
