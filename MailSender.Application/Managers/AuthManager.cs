using AutoMapper;
using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Domain.DTOs;

namespace MailSender.Application.Managers
{
    /// <summary>
    /// Handles application registration and authentication logic.
    /// </summary>
    public class AuthManager : IAuthManager
    {
        /// <summary>
        /// Temporary static password used for all registered clients.
        /// ⚠️ This is for development/demo purposes only and should be replaced in production.
        /// </summary>
        private const string PASSWORD = "test";

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="AuthManager"/>.
        /// </summary>
        /// <param name="tokenService">Service responsible for JWT token creation.</param>
        public AuthManager(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// In-memory list of registered client applications.
        /// ⚠️ This is a temporary substitute for a database – for demo or development purposes only.
        /// </summary>
        private static readonly List<ClientApp> Clients = new()
        {
            new ClientApp { AppId = "app1", AppName = "secret1", Pass = PASSWORD },
            new ClientApp { AppId = "app2", AppName = "secret2", Pass = PASSWORD }
        };

        /// <summary>
        /// Registers a new client application if its AppId and AppName are unique.
        /// Returns a result containing the registered app data and JWT token on success.
        /// </summary>
        /// <param name="registerDto">DTO containing the client app registration data.</param>
        /// <returns>
        /// A <see cref="Result{RegisteredDto}"/> containing token and registration info,
        /// or an error result if app already exists.
        /// </returns>
        public async Task<Result<RegisteredDto>> RegisterApplicationAsync(RegisterAppDto registerDto)
        {
            var signingKeyIsEmptyOrNull = String.IsNullOrEmpty(registerDto.SigningJwtKey);
            if (signingKeyIsEmptyOrNull)
                return Error.Failure("This signingKey is empty or nul.");

            var appIdExist = Clients.Any(x => (x.AppId == registerDto.AppId));
            if (appIdExist)
                return Error.Conflict("This appName is already exist.");

            var appNameExist = Clients.Any(x => (x.AppName == registerDto.AppName));
            if (appNameExist)
                return Error.Conflict("This appName is already exist.");

            var newClientApp = _mapper.Map<ClientApp>(registerDto);

            var token = _tokenService.CreateAccessToken(newClientApp, registerDto.SigningJwtKey!);

            var result = _mapper.Map<RegisteredDto>(newClientApp);
            result.Key = token;

            return result;
        }
    }
}
