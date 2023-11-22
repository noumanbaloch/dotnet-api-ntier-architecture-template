using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.Auth
{
    public interface IAuthFacadeService
    {
        Task<GenericResponse<UserResponseDto>> Register(RegisterRequestDto requestDto);
        Task<GenericResponse<UserResponseDto>> Login(LoginRequestDto requestDto);
        Task<GenericResponse<UserResponseDto>> ChangePassword(ChangePasswordRequestDto requestDto);
        Task<GenericResponse<UserResponseDto>> ForgotPassword(ForgotPasswordRequestDto requestDto);
        Task<GenericResponse<bool>> CheckForUserExists(string userName);
        Task<GenericResponse<UserResponseDto>> UpdateProfile(UpdateProfileRequestDto requestDto);
    }
}
