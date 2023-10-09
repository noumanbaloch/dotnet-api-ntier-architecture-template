using Breeze.Models.Dtos.Email.Request;

namespace Breeze.Services.Email;
public interface IEmailService
{
    Task SendEmailAsync<T>(EmailRequestDto<T> emailRequestDto);
    string GetOTPEmailTemplateBasedOnUseCase(string useCaseName);
}
