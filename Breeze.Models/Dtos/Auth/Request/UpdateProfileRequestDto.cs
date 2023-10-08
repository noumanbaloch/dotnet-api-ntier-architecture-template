using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Auth.Request;

public class UpdateProfileRequestDto
{
    [Required(ErrorMessage = RequestValidationMessages.FIRST_NAME_REQUIRED_MSG)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = RequestValidationMessages.LAST_NAME_REQUIRED_MSG)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = RequestValidationMessages.PHONE_NUMBER_REQUIRED_MSG)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}$", ErrorMessage = RequestValidationMessages.VALID_EMAIL_ADDRESS_REQUIRED_MSG)]
    public string UserName { get; set; } = string.Empty;
}