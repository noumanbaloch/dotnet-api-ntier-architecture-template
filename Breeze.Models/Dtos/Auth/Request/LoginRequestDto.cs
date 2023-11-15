using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Auth.Request;
public class LoginRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}$", ErrorMessage = RequestValidationMessages.VALID_EMAIL_ADDRESS_REQUIRED_MSG)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.PASSWORD_REQUIRED_MSG)]
    public required string Password { get; set; }
    public required string OTPCode { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.OTP_USE_CASE_REQUIRED_MSG)]
    public required string OTPUseCase { get; set; }
}
