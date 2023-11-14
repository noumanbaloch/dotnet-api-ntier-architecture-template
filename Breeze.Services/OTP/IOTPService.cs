using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Dtos.OTP.Response;

namespace Breeze.Services.OTP;
public interface IOTPService
{
    Task<bool> IsValideOTP(VerifyOTPRequestDto verifyOtpRequestDto);
    Task SendOTPEmail(OTPEmailRequestDto emailOtpRequestDto);
    Task SaveOTP(SaveOTPRequestDto saveOtpResponseDto);
    OTPResponseDto GenerateOTP(GenerateOTPRequestDto genOtpRequestDto);
    Task InvalidateExistingOTPs(string userName);
}

