using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Auth.Request;
public class RegisterRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.FIRST_NAME_REQUIRED_MSG)]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.LAST_NAME_REQUIRED_MSG)]
    public required string LastName { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}$", ErrorMessage = RequestValidationMessages.VALID_EMAIL_ADDRESS_REQUIRED_MSG)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.PASSWORD_REQUIRED_MSG)]
    public required string Password { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.PHONE_NUMBER_REQUIRED_MSG)]
    public required string PhoneNumber { get; set; }

    public int DisciplineId { get; set; }

    [Required]
    [Range(typeof(bool), "true", "true", ErrorMessage = RequestValidationMessages.ACCEPTED_TERMS_AND_CONDITIONS_REQUIRED_MSG)]
    public bool AcceptedTermsAndConditions { get; set; }

    public required string OTPCode { get; set; }

    [Required(ErrorMessage = RequestValidationMessages.OTP_USE_CASE_REQUIRED_MSG)]
    public required string OTPUseCase { get; set; }

}