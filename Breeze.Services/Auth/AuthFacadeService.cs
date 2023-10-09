using AutoMapper;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Entities;
using Breeze.Models.GenericResponses;
using Breeze.Models.ModelMapping;
using Breeze.Services.ClaimResolver;
using Breeze.Services.HttpHeader;
using Breeze.Services.OTP;
using Breeze.Services.TokenService;
using Breeze.Utilities;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Breeze.Services.Auth;

public class AuthFacadeService : IAuthFacadeService
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IOTPService _otpService;
    private readonly IMapper _mapper;
    private readonly IHttpHeaderService _httpHeaderService;
    private readonly IAuthService _authService;
    private readonly IClaimResolverService _claimResolverService;

    public AuthFacadeService(SignInManager<UserEntity> signInManager,
        UserManager<UserEntity> userManager,
        ITokenService tokenService,
        IOTPService otpService,
        IMapper mapper,
        IHttpHeaderService httpHeaderService,
        IAuthService authService,
        IClaimResolverService claimResolverService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _otpService = otpService;
        _mapper = mapper;
        _httpHeaderService = httpHeaderService;
        _authService = authService;
        _claimResolverService = claimResolverService;
    }

    public async Task<GenericResponse<UserResponseDto>> Register(RegisterRequestDto requestDto)
    {
        if (await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.USER_ALREADY_EXIST, ApiStatusCodes.USER_ALREADY_EXIST);
        }

        if (string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDTO>(otpResponseDto));
            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        if (!await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto)))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_OTP, ApiStatusCodes.INVALID_OTP);
        }

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = requestDto.ToUserEntity(_httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString());

            while (await _authService.UserhandleAlreadyExist(user.UserHandle, user.UserName!))
            {
                user.UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}";
            }

            var userResult = await _userManager.CreateAsync(user, requestDto.Password);

            if (!userResult.Succeeded)
            {
                return GenericResponse<UserResponseDto>.Failure(userResult.Errors.FirstOrDefault()!.Description!, ApiStatusCodes.UNABLE_TO_COMPLETE_PROCESS);
            }

            var assignedRoles = await _userManager.AddToRoleAsync(user, UserRoles.ADMIN_ROLE);

            if (!assignedRoles.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return GenericResponse<UserResponseDto>.Failure(assignedRoles.Errors.FirstOrDefault()!.Description, ApiStatusCodes.UNABLE_TO_COMPLETE_PROCESS);
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

            scope.Complete();

            return GenericResponse<UserResponseDto>.Success(user.ToUserResponseDto(token), $"{ApiResponseMessages.WELCOME_MESSAGE}{user.FirstName}", ApiStatusCodes.WELCOME_MESSAGE);
        }
    }

    public async Task<GenericResponse<UserResponseDto>> Login(LoginRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);
        }

        var deviceIdIsTrusted = await _authService.ValidateTrustedDevice(user.UserName!, _httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString());
        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);

        if (!signInResult.Succeeded)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);
        }

        if (!deviceIdIsTrusted && string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDTO>(otpResponseDto));
            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        if (!deviceIdIsTrusted && !await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto)))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_OTP, ApiStatusCodes.INVALID_OTP);
        }

        if (!deviceIdIsTrusted)
        {
            await _authService.UpdateDevice(user.UserName!);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return GenericResponse<UserResponseDto>.Success(user.ToUserResponseDto(token), $"{ApiResponseMessages.WELCOME_BACK_MESSAGE}{user.FirstName}", ApiStatusCodes.WELCOME_BACK_MESSAGE);
    }

    public async Task<GenericResponse<UserResponseDto>> ChangePassword(ChangePasswordRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);
        }

        var isValidPassword = await _signInManager.CheckPasswordSignInAsync(user, requestDto.CurrentPassword, false);

        if (!isValidPassword.Succeeded)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.PASSWORD_IS_INVALID, ApiStatusCodes.PASSWORD_IS_INVALID);
        }

        IdentityResult changePassword = await ChangePassword(user, requestDto.NewPassword);

        if (!changePassword.Succeeded)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, ApiStatusCodes.SOMETHING_WENT_WRONG);
        }

        var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, requestDto.NewPassword, false);

        if (!checkPassword.Succeeded)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.PASSWORD_IS_INVALID, ApiStatusCodes.PASSWORD_IS_INVALID);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return GenericResponse<UserResponseDto>.Success(user.ToUserResponseDto(token), ApiResponseMessages.PASSWORD_CHANGED, ApiStatusCodes.PASSWORD_CHANGED);
    }

    public async Task<GenericResponse<UserResponseDto>> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);
        }

        if (string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDTO>(otpResponseDto));

            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        var isValidOTP = await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto));

        if (!isValidOTP)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_OTP, ApiStatusCodes.INVALID_OTP);
        }

        await ChangePassword(user, requestDto.NewPassword);

        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return GenericResponse<UserResponseDto>.Success(user.ToUserResponseDto(token), $"{ApiResponseMessages.WELCOME_BACK_MESSAGE}{user.FirstName}", ApiStatusCodes.WELCOME_BACK_MESSAGE);
    }

    public async Task<GenericResponse<bool>> CheckForUserExists(string userName)
    {
        if (!await _authService.UserExists(userName))
        {
            return GenericResponse<bool>.Failure(false, ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST);
        }

        return GenericResponse<bool>.Success(true, ApiResponseMessages.USER_ALREADY_EXIST, ApiStatusCodes.USER_ALREADY_EXIST);
    }

    public async Task<GenericResponse> VerifyEmail(VerifyEmailRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse.Failure(ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST);
        }

        if ((!user.EmailConfirmed) && string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDTO>(otpResponseDto));
            return GenericResponse.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        if ((!user.EmailConfirmed) && !await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto)))
        {
            return GenericResponse.Failure(ApiResponseMessages.INVALID_OTP, ApiStatusCodes.INVALID_OTP);
        }

        if (!user.EmailConfirmed)
        {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

        return GenericResponse.Success(ApiResponseMessages.EMAIL_VERIFIED, ApiStatusCodes.EMAIL_VERIFIED);
    }

    public async Task<GenericResponse<UserResponseDto>> UpdateProfile(UpdateProfileRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST);
        }

        _mapper.Map(requestDto, user);

        while (await _authService.UserhandleAlreadyExist(user.UserHandle, user.UserName!))
        {
            user.UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}";
        }

        user.ModifiedBy = _claimResolverService.GetLoggedInUsername()!;
        user.ModifiedDate = Helper.GetCurrentDate();

        await _userManager.UpdateAsync(user);


        var roles = await _userManager.GetRolesAsync(user);

        var token = _tokenService.GenerateToken(user.ToCreateTokenRequesDto(roles.ToList()));

        return GenericResponse<UserResponseDto>.Success(user.ToUserResponseDto(token), ApiResponseMessages.PROFILE_UPDATED_SUCCESSFULLY, ApiStatusCodes.PROFILE_UPDATED_SUCCESSFULLY);
    }


    #region private Methods
    private async Task<IdentityResult> ChangePassword(UserEntity user, string newPassword)
    {
        await _signInManager.SignOutAsync();
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        return await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
    }
    #endregion
}