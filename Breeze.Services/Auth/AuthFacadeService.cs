using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.Auth;

public class AuthFacadeService : IAuthFacadeService
{
    private readonly IAuthService _authService;

    public AuthFacadeService(
        IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<GenericResponse<UserResponseDto>> Register(RegisterRequestDto requestDto)
    {
        var result = await _authService.Register(requestDto);

        if (result.Item1 == ResponseEnums.UserRegisteredSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2, result!.Item2!.FirstName);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<UserResponseDto>> Login(LoginRequestDto requestDto)
    {
        var result = await _authService.Login(requestDto);

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
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2, result!.Item2!.FirstName);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
    }

    public async Task<GenericResponse<UserResponseDto>> ForgotPassword(ForgotPasswordRequestDto requestDto)
    {
        var result = await _authService.ForgotPassword(requestDto);

        if (result.Item1 == ResponseEnums.UserLoginSuccessfully)
        {
            return GenericResponseHelper.GenerateEnumToGenericResponse(result.Item1, result.Item2);
        }

        return GenericResponseHelper.GenerateEnumToGenericResponse<UserResponseDto>(result.Item1, null);
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
        var result = await _authService.VerifyEmail(requestDto);
        return GenericResponseHelper.GenerateEnumToGenericResponse(result);
    }


}