﻿using MailSender.Application.Managers.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MailSender.Api.Controllers
{
    /// <summary>
    /// Controller for managing email sending operations and retrieving mail logs.
    /// </summary>
    [Route("api/mail")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailManager _mailManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailController"/> class.
        /// </summary>
        /// <param name="mailManager">Service responsible for handling mail sending and logging logic.</param>
        public MailController(IMailManager mailManager)
        {
            _mailManager = mailManager;
        }

        /// <summary>
        /// Sends an email message on behalf of the authenticated application.
        /// </summary>
        /// <remarks>
        /// This endpoint sends an email using the data provided in the request body.
        /// The email is sent on behalf of the application identified by claims "app_id" and "app_name".
        /// 
        /// The client must be authenticated to access this endpoint.
        /// The request body must include a valid <see cref="MailDto"/> object representing the email content.
        /// 
        /// On failure, the error is logged into the database.
        /// </remarks>
        /// <param name="sendMailDto">The email message details sent by the client application.</param>
        /// <response code="200">Email sent successfully.</response>
        /// <response code="400">Invalid input or validation failure.</response>
        /// <response code="401">Client is not authenticated.</response>
        /// <response code="500">Unexpected server error occurred.</response>
        /// <example>
        /// POST /api/mail/send
        /// Content-Type: application/json
        /// 
        /// {
        ///   "to": "recipient@example.com",
        ///   "subject": "Test Email",
        ///   "body": "&lt;h1&gt;Hello&lt;/h1&gt;&lt;p&gt;This is a test email.&lt;/p&gt;"
        /// }
        /// </example>
        [Authorize]
        [HttpPost("send")]
        [ProducesResponseType(typeof(SendedMailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendAsync([FromBody] MailDto sendMailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claimsPrincipal = HttpContext.User;
            var appId = claimsPrincipal.FindFirst("app_id")?.Value;
            var appName = claimsPrincipal.FindFirst("app_name")?.Value;

            var result = await _mailManager.SendAsync(appId, appName, sendMailDto);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else if (result.Error != null)
            {
                if (result.Error.errorType == ErrorType.Unauthentication)
                {
                    return Problem(
                        statusCode: 401,
                        title: "Unauthorized",
                        detail: result.Error.Description
                    );
                }

                return Problem(
                    statusCode: 500,
                    title: "InternalServerError",
                    detail: result.Error.Description
                );
            }

            return Problem(
                statusCode: 500,
                title: "InternalServerError",
                detail: "An unexpected error occurred."
            );
        }

        /// <summary>
        /// Retrieves mail logs for the authenticated application.
        /// </summary>
        /// <remarks>
        /// This endpoint returns the list of mail sending logs filtered by the authenticated application's "app_id" claim.
        /// The client must be authenticated to access this endpoint.
        /// </remarks>
        /// <response code="200">Returns the list of mail logs.</response>
        /// <response code="400">Invalid input or validation failure.</response>
        /// <response code="401">Client is not authenticated.</response>
        /// <response code="500">Unexpected server error occurred.</response>
        [Authorize]
        [HttpGet("get-log")]
        [ProducesResponseType(typeof(List<MailLog>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLogAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claimsPrincipal = HttpContext.User;
            var appId = claimsPrincipal.FindFirst("app_id")?.Value;

            var result = await _mailManager.GetLogsAsync(appId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else if (result.Error != null)
            {
                if (result.Error.errorType == ErrorType.Unauthentication)
                {
                    return Problem(
                        statusCode: 401,
                        title: "Unauthorized",
                        detail: result.Error.Description
                    );
                }

                return Problem(
                    statusCode: 500,
                    title: "InternalServerError",
                    detail: result.Error.Description
                );
            }

            return Problem(
                statusCode: 500,
                title: "InternalServerError",
                detail: "An unexpected error occurred."
            );
        }
    }
}
