
using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.OTP.Request;

public class VerifyOTPRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.OTP_CODE_REQUIRED_MSG)]
    public required string OTPCode { get; set; }

    [Required]
    public required string OTPUseCase { get; set; }
}
