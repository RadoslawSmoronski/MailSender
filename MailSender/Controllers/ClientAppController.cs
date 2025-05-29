using AutoMapper;
using MailSender.Application.Managers.Interfaces;
using MailSender.Common.Result;
using MailSender.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

namespace MailSender.Api.Controllers
{
    /// <summary>
    /// Controller for managing client application registration.
    /// </summary>
    [Route("api/client-app")]
    [ApiController]
    public class ClientAppController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthManager _authManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAppController"/> class.
        /// </summary>
        /// <param name="config">Configuration settings.</param>
        /// <param name="authManager">Service for authentication and registration logic.</param>
        /// <param name="mapper">AutoMapper instance for mapping DTOs.</param>
        public ClientAppController(IConfiguration config, IAuthManager authManager, IMapper mapper)
        {
            _config = config;
            _authManager = authManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new client application.
        /// </summary>
        /// <remarks>
        /// This endpoint registers a new client application with the system.
        /// The request must include all required registration data in the body as JSON.
        /// 
        /// During registration, the server uses a configured JWT signing key and passes it along
        /// with the client data to the authentication manager.
        /// </remarks>
        /// <param name="registerDto">The registration data sent by the client application.</param>
        /// <returns>
        /// Returns a success response with registration result, or a problem response in case of error.
        /// </returns>
        /// <response code="200">Application registered successfully.</response>
        /// <response code="400">Invalid input or validation failure.</response>
        /// <response code="409">Conflict – application with this data already exists.</response>
        /// <response code="500">Unexpected server error occurred.</response>
        /// <example>
        /// POST /api/client-app/register
        /// Content-Type: application/json
        /// 
        /// {
        ///   "name": "MyApp",
        ///   "email": "contact@myapp.com",
        ///   "password": "StrongPassword123!"
        /// }
        /// </example>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var signingKey = _config["JWT:SigningKey"];

            var registerAppDto = _mapper.Map<RegisterAppDto>(registerDto);
            registerAppDto.SigningJwtKey = signingKey;

            var result = await _authManager.RegisterApplicationAsync(registerAppDto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            if (result.Error != null)
            {
                var errorType = result.Error.errorType;
                var errorMessage = result.Error.Description;

                if (errorType == ErrorType.Failure)
                {
                    return Problem(
                        statusCode: 500,
                        title: "InternalServerError",
                        detail: errorMessage
                    );
                }
                else if (errorType == ErrorType.Conflict)
                {
                    return Problem(
                        statusCode: 409,
                        title: "Conflict",
                        detail: errorMessage
                    );
                }
            }

            return Problem(
                statusCode: 500,
                title: "InternalServerError",
                detail: "An unexpected error occurred."
            );
        }
    }
}
