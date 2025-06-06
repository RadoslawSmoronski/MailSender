using AutoMapper;
using MailSender.Application.Managers.Interfaces;
using MailSender.Application.Services.Interfaces;
using MailSender.Common.Result;
using MailSender.Contracts.DTOs;
using MailSender.Infrastructure.Database.Repository;

/// <summary>
/// Handles client application registration and authentication logic.
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
    private readonly IRepository<ClientApp> _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthManager"/> class.
    /// </summary>
    /// <param name="tokenService">Service responsible for JWT token creation.</param>
    /// <param name="mapper">Mapper used to transform between DTOs and domain entities.</param>
    /// <param name="repository">Generic repository for <see cref="ClientApp"/> entity.</param>
    public AuthManager(ITokenService tokenService, IMapper mapper, IRepository<ClientApp> repository)
    {
        _tokenService = tokenService;
        _mapper = mapper;
        _repository = repository;
    }

    /// <summary>
    /// Registers a new client application if both AppId and AppName are unique.
    /// On success, creates a JWT token and persists the client application to the database.
    /// </summary>
    /// <param name="registerDto">DTO containing the client application's registration data.</param>
    /// <returns>
    /// A <see cref="Result{RegisteredDto}"/> containing token and client registration info,
    /// or an error result if the AppId or AppName is already in use or input is invalid.
    /// </returns>
    public async Task<Result<RegisteredDto>> RegisterApplicationAsync(RegisterAppDto registerDto)
    {
        var signingKeyIsEmptyOrNull = string.IsNullOrEmpty(registerDto.SigningJwtKey);
        if (signingKeyIsEmptyOrNull)
            return Error.Failure("The signing key is empty or null.");

        try
        {
            var appIdExist = await _repository.AnyAsync(x => x.AppId == registerDto.AppId);
            if (appIdExist)
                return Error.Conflict("This AppId is already in use.");

            var appNameExist = await _repository.AnyAsync(x => x.AppName == registerDto.AppName);
            if (appNameExist)
                return Error.Conflict("This AppName is already in use.");

            var newClientApp = _mapper.Map<SimpleClientAppDto>(registerDto);

            var token = _tokenService.CreateAccessToken(newClientApp, registerDto.SigningJwtKey!);

            var result = _mapper.Map<RegisteredDto>(newClientApp);
            result.Key = token;

            var newAppClient = new ClientApp()
            {
                AppId = registerDto.AppId,
                AppName = registerDto.AppName,
                Pass = registerDto.Pass
            };

            await _repository.AddAsync(newAppClient);
            await _repository.SaveChangesAsync();

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }
    }
}
