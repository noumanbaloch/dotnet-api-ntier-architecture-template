
using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.OTP.Request;

public class VerifyOTPRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = RequestValidationMessages.OTP_CODE_REQUIRED_MSG)]
    public string OTPCode { get; set; } = string.Empty;

    [Required]
    public string OTPUseCase { get; set; } = string.Empty;
}
