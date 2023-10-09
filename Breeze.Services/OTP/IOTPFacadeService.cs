using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.OTP;
public interface IOTPFacadeService
{
    Task<GenericResponse<bool>> GenerateOTP(GenerateOTPRequestDto requestDto);
    Task<GenericResponse<bool>> VerifyOTP(VerifyOTPRequestDto requestDto);
}
