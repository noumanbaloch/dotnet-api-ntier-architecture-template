using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Email.Request;
public class OTPEmailRequestDTO
{
    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string OTPCode { get; set; } = string.Empty;

    [Required]
    public string OTPUseCase { get; set; } = string.Empty;
}