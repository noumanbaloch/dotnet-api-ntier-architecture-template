using AutoMapper;
using Breeze.DbCore.UnitOfWork;
using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Entities;
using Breeze.Models.ModelMapping;
using Breeze.Services.Cache;
using Breeze.Services.ClaimResolver;
using Breeze.Services.HttpHeader;
using Breeze.Services.TokenService;
using Breeze.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Breeze.Services.Auth;
public class AuthService(IUnitOfWork _unitOfWork,
    IHttpHeaderService _httpHeaderService,
    IMapper _mapper,
    ICacheService _cacheService,
    SignInManager<UserEntity> _signInManager,
    UserManager<UserEntity> _userManager,
    IClaimResolverService _claimResolverService,
    ITokenService _tokenService) : IAuthService
{ 
    public async Task<(ResponseEnums, UserResponseDto?)> Register(RegisterRequestDto requestDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = requestDto.ToUserEntity(_httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString());

            while (await UserhandleAlreadyExist(user.UserHandle, user.UserName!))
            {
                user.UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}";
            }

            var userResult = await _userManager.CreateAsync(user, requestDto.Password);

            if (!userResult.Succeeded)
            {
                return (ResponseEnums.UnableToCompleteProcess, null);
            }


            var assignedRoles = await _userManager.AddToRoleAsync(user, UserRoles.ADMIN_ROLE);

            if (!assignedRoles.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return (ResponseEnums.UnableToCompleteProcess, null);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));


            scope.Complete();

            return (ResponseEnums.UserRegisteredSuccessfully, user.ToUserResponseDto(token));
        }
    }

    public async Task<(ResponseEnums, UserResponseDto?)> Login(UserEntity user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return (ResponseEnums.UserLoginSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> ChangePassword(ChangePasswordRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await UserExists(requestDto.UserName))
        {
            return (ResponseEnums.InvalidUsernamePassword, null);
        }

        var isValidPassword = await _signInManager.CheckPasswordSignInAsync(user, requestDto.CurrentPassword, false);

        if (!isValidPassword.Succeeded)
        {
            return (ResponseEnums.InvalidPassword, null);
        }

        IdentityResult changePassword = await ChangeUserPassword(user, requestDto.NewPassword);

        if (!changePassword.Succeeded)
        {
            return (ResponseEnums.SomethingWentWrong, null);
        }

        var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, requestDto.NewPassword, false);

        if (!checkPassword.Succeeded)
        {
            return (ResponseEnums.InvalidPassword, null);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return (ResponseEnums.PasswordChangedSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await UserExists(requestDto.UserName))
        {
            return (ResponseEnums.InvalidUsernamePassword, null);
        }

        await ChangeUserPassword(user, requestDto.NewPassword);

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return (ResponseEnums.UserLoginSuccessfully, user.ToUserResponseDto(token));
    }

    public async Task<(ResponseEnums, UserResponseDto?)> UpdateProfile(UpdateProfileRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
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
        user.ModifiedDate = Helper.GetCurrentDate();

        await _userManager.UpdateAsync(user);


        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

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
            entity.ModifiedDate = Helper.GetCurrentDate();
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
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null || !await UserExists(userName))
        {
            return null;
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!signInResult.Succeeded)
        {
            return null;
        }

        return user;
    }
    #region private Methods

    private async Task<IdentityResult> ChangeUserPassword(UserEntity user, string newPassword)
    {
        await _signInManager.SignOutAsync();
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        return await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
    }
    #endregion
}
