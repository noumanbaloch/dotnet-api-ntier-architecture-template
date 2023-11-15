using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Auth.Request
{
    public class ChangePasswordRequestDto
    {
        [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}$", ErrorMessage = RequestValidationMessages.VALID_EMAIL_ADDRESS_REQUIRED_MSG)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = RequestValidationMessages.CURRENT_PASS_REQUIRED_MSG)]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = RequestValidationMessages.NEW_PASS_REQUIRED_MSG)]
        public required string NewPassword { get; set; }
    }
}
