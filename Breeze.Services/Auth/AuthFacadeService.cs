using AutoMapper;
using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.GenericResponses;
using Breeze.Services.HttpHeader;
using Breeze.Services.OTP;

namespace Breeze.Services.Auth;

public class AuthFacadeService(IAuthService _authService,
        IOTPService _otpService,
        IMapper _mapper,
        IHttpHeaderService _httpHeaderService) : IAuthFacadeService
{ 
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
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDto>(otpResponseDto));
            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        if (!await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto)))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE);
        }

        var result = await _authService.Register(requestDto);

        if (result.Item1 == ResponseEnums.UserRegisteredSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2, result!.Item2!.FirstName);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<UserResponseDto>> Login(LoginRequestDto requestDto)
    {
        var user = await _authService.CheckForValidUserNamePassword(requestDto.UserName, requestDto.Password);

        if (user == null)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);

        }

        var deviceIdIsTrusted = await _authService.ValidateTrustedDevice(user.UserName!, _httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString());
        if (!deviceIdIsTrusted && string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDto>(otpResponseDto));
            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }
        if (!deviceIdIsTrusted && !await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto)))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE);
        }

        if (!deviceIdIsTrusted)
        {
            await _authService.UpdateDevice(user.UserName!);
        }

        var result = await _authService.Login(user);

        if (result.Item1 == ResponseEnums.UserLoginSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2, result!.Item2!.FirstName);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<UserResponseDto>> ChangePassword(ChangePasswordRequestDto requestDto)
    {
        var result = await _authService.ChangePassword(requestDto);

        if (result.Item1 == ResponseEnums.PasswordChangedSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<UserResponseDto>> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        if (!await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD);
        }

        if (string.IsNullOrWhiteSpace(requestDto.OTPCode))
        {
            await _otpService.InvalidateExistingOTPs(requestDto.UserName);
            var otpResponseDto = _otpService.GenerateOTP(_mapper.Map<GenerateOTPRequestDto>(requestDto));
            await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
            await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDto>(otpResponseDto));

            return GenericResponse<UserResponseDto>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
        }

        var isValidOTP = await _otpService.IsValideOTP(_mapper.Map<VerifyOTPRequestDto>(requestDto));

        if (!isValidOTP)
        {
            return GenericResponse<UserResponseDto>.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE);
        }

        var result = await _authService.ForgotPassword(requestDto);

        if (result.Item1 == ResponseEnums.UserLoginSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2, result!.Item2!.FirstName);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<bool>> CheckForUserExists(string userName)
    {
        if (!await _authService.UserExists(userName))
        {
            return GenericResponse<bool>.Failure(false, ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST);
        }

        return GenericResponse<bool>.Success(true, ApiResponseMessages.USER_ALREADY_EXIST, ApiStatusCodes.USER_ALREADY_EXIST);
    }

    public async Task<GenericResponse<UserResponseDto>> UpdateProfile(UpdateProfileRequestDto requestDto)
    {
        var result = await _authService.UpdateProfile(requestDto);

        if (result.Item1 == ResponseEnums.ProfileUpdatedSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }
}