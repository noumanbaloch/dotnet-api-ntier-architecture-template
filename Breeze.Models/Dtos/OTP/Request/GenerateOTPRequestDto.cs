using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.OTP.Request;
public class GenerateOTPRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    public required string UserName { get; set; }

    [Required]
    public required string OTPUseCase { get; set; }
}
