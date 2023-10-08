﻿using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Auth.Request
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = RequestValidationMessages.USERNAME_REQUIRED_MSG)]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,6}$", ErrorMessage = RequestValidationMessages.VALID_EMAIL_ADDRESS_REQUIRED_MSG)]
        public string UserName { get; set; } = string.Empty;
        public string OTPCode { get; set; } = string.Empty;
        public string OTPUseCase { get; set; } = string.Empty;
       
        [Required(ErrorMessage = RequestValidationMessages.NEW_PASS_REQUIRED_MSG)]
        public string NewPassword { get; set; } = string.Empty;
    }
}