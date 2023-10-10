using Breeze.Models.ApplicationEnums;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Entities;

namespace Breeze.Services.Auth;
public interface IAuthService
{
    Task<(ResponseEnums, UserResponseDto?)> Register(RegisterRequestDto requestDto);
    Task<(ResponseEnums, UserResponseDto?)> Login(LoginRequestDto requestDto);
    Task<(ResponseEnums, UserResponseDto?)> ChangePassword(ChangePasswordRequestDto requestDto);
    Task<(ResponseEnums, UserResponseDto?)> ForgotPassword(ForgotPasswordRequestDto requestDto);
    Task<ResponseEnums> VerifyEmail(VerifyEmailRequestDto requestDto);
    Task<(ResponseEnums, UserResponseDto?)> UpdateProfile(UpdateProfileRequestDto requestDto);
    Task UpdateDevice(string userName);
    Task<bool> UserExists(string userName);
    Task<UserEntity?> GetUserByUsername(string userName);
    Task<bool> ValidateTrustedDevice(string userName, string deviceId);
    Task<bool> UserhandleAlreadyExist(string userHandle, string userName);

}
