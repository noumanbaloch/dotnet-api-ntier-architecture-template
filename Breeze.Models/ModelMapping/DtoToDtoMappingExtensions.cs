using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Dtos.OTP.Response;

namespace Breeze.Models.ModelMapping;
public static class DtoToDtoMappingExtensions
{
    public static OTPResponseDto ToOTPResponseDto(this GenerateOTPRequestDto requestDto, string otpCode)
    {
        return new OTPResponseDto
        {
            UserName = requestDto.UserName,
            OTPUseCase = requestDto.OTPUseCase,
            OTPCode = otpCode
        };
    }
}