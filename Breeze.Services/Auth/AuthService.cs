using AutoMapper;
using Breeze.DbCore.UnitOfWork;
using Breeze.Identity;
using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Entities;
using Breeze.Models.ModelMapping;
using Breeze.Services.Cache;
using Breeze.Services.ClaimResolver;
using Breeze.Services.HttpHeader;
using Breeze.Services.Token;
using Breeze.Utilities;
using System.Transactions;

namespace Breeze.Services.Auth;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpHeaderService _httpHeaderService;
    private readonly ICacheService _cacheService;
    private readonly IClaimResolverService _claimResolverService;
    private readonly ITokenService _tokenService;
    private readonly IIdentityService _identityService;

    public AuthService(IUnitOfWork unitOfWork,
        IHttpHeaderService httpHeaderService,
        IMapper mapper,
        ICacheService cacheService,
        IClaimResolverService claimResolverService,
        ITokenService tokenService,
        IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _httpHeaderService = httpHeaderService;
        _mapper = mapper;
        _cacheService = cacheService;
        _claimResolverService = claimResolverService;
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<(ResponseEnums, UserResponseDto?)> Register(RegisterRequestDto requestDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = requestDto.ToUserEntity(_httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString());

            while (await UserhandleAlreadyExist(user.UserHandle, user.UserName!))
            {
                user.UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}";
            }

            if (!await _identityService.CreateUser(user, requestDto.Password))
            {
                return (ResponseEnums.UnableToCompleteProcess, null);
            }

            if (!await _identityService.AddUserRole(user, UserRoles.ADMIN_ROLE))
            {
                await _identityService.DeleteUser(user);
                return (ResponseEnums.UnableToCompleteProcess, null);
            }

            var roles = await _identityService.GetUserRoles(user);
            var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles));


            scope.Complete();

            return (ResponseEnums.UserRegisteredSuccessfully, user.ToUserResponseDto(token));
        }
    }

    public async Task<(ResponseEnums, UserResponseDto?)> Login(UserEntity user)
    {
        var roles = await _identityService.GetUserRoles(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles));

        return (ResponseEnums.UserLoginSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> ChangePassword(ChangePasswordRequestDto requestDto)
    {
        var user = await _identityService.FindByUserName(requestDto.UserName);
        if (user is null || !await UserExists(requestDto.UserName))
        {
            return (ResponseEnums.InvalidUsernamePassword, null);
        }

        if (!await _identityService.ValidateUserPassword(user, requestDto.CurrentPassword))
        {
            return (ResponseEnums.InvalidPassword, null);
        }

        if (!await _identityService.ChangeUserPassword(user, requestDto.NewPassword))
        {
            return (ResponseEnums.SomethingWentWrong, null);
        }

        if (!await _identityService.ValidateUserPassword(user, requestDto.NewPassword))
        {
            return (ResponseEnums.InvalidPassword, null);
        }

        var roles = await _identityService.GetUserRoles(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles));

        return (ResponseEnums.PasswordChangedSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        var user = await _identityService.FindByUserName(requestDto.UserName);
        if (user is null || !await UserExists(requestDto.UserName))
        {
            return (ResponseEnums.InvalidUsernamePassword, null);
        }

        await _identityService.ChangeUserPassword(user, requestDto.NewPassword);

        var roles = await _identityService.GetUserRoles(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles));

        return (ResponseEnums.UserLoginSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> UpdateProfile(UpdateProfileRequestDto requestDto)
    {
        var user = await _identityService.FindByUserName(requestDto.UserName);
        if (user is null || !await UserExists(requestDto.UserName))
        {
            return (ResponseEnums.UserDoesNotExist, null);
        }

        _mapper.Map(requestDto, user);

        while (await UserhandleAlreadyExist(user.UserHandle, user.UserName!))
        {
            user.UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}";
        }

        user.ModifiedBy = _claimResolverService.GetLoggedInUsername();
        user.ModifiedDate = DateTime.Now;

        await _identityService.UpdateUser(user);


        var roles = await _identityService.GetUserRoles(user);
        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles));

        return (ResponseEnums.ProfileUpdatedSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task UpdateDevice(string userName)
    {
        var repo = _unitOfWork.GetRepository<UserEntity>();
        var entity = await repo.FindByFirstOrDefaultAsync(x => x.UserName!.ToLower() == userName.ToLower() &&
        !x.Deleted);

        var newDeviceId = _httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString();
        if (entity is not null)
        {
            _cacheService.RemoveData($"{CacheKeys.TRUSTED_DEVICE}{userName}{entity.TrustedDeviceId}");
            _cacheService.SetData($"{CacheKeys.TRUSTED_DEVICE}{userName}{newDeviceId}", newDeviceId);

            entity.TrustedDeviceId = newDeviceId;
            entity.ModifiedBy = userName;
            entity.ModifiedDate = DateTime.Now;
            repo.Update(entity);
        }

        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> UserExists(string userName) => await _unitOfWork.GetRepository<UserEntity>()
             .AnyAsync(x => x.UserName!.ToLower() == userName.ToLower() &&
             !x.Deleted);

    public async Task<UserEntity?> GetUserByUsername(string userName)
    {
        return await _unitOfWork.GetRepository<UserEntity>().FindByFirstOrDefaultAsync(x => x.UserName!.ToLower() == userName.ToLower() &&
        !x.Deleted);
    }

    public async Task<bool> ValidateTrustedDevice(string userName, string deviceId)
    {
        var cacheKey = $"{CacheKeys.TRUSTED_DEVICE}{userName}{deviceId}";
        var cacheTrusted = _cacheService.GetData<string>(cacheKey);
        if (string.IsNullOrWhiteSpace(cacheTrusted))
        {
            var user = await GetUserByUsername(userName);
            if (user is null || user.TrustedDeviceId != deviceId)
            {
                return false;
            }

            if (!_cacheService.Exists(cacheKey))
            {
                _cacheService.SetData(cacheKey, user.TrustedDeviceId);
            }
            return true;
        }
        return true;
    }

    public async Task<bool> UserhandleAlreadyExist(string userHandle, string userName)
        => await _unitOfWork.GetRepository<UserEntity>().AnyAsync(x => x.UserHandle == userHandle &&
        x.UserName != userName &&
        !x.Deleted);


    public async Task<UserEntity?> CheckForValidUserNamePassword(string userName, string password)
    {
        var user = await _identityService.FindByUserName(userName);
        if (user is null || !await UserExists(userName))
        {
            return default;
        }

        if (!await _identityService.ValidateUserPassword(user, password))
        {
            return default;
        }

        return user;
    }
}
