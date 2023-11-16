using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Dtos.OTP.Response;

namespace Breeze.Services.OTP;
public interface IOTPService
{
    Task<bool> IsValideOTP(VerifyOTPRequestDto requestDto);
    Task SendOTPEmail(OTPEmailRequestDto requestDto);
    Task SaveOTP(SaveOTPRequestDto requestDto);
    OTPResponseDto GenerateOTP(GenerateOTPRequestDto requestDto);
    Task InvalidateExistingOTPs(string userName);
}

