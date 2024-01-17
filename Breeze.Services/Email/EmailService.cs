using Breeze.Models.Configurations;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Email.Request;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Breeze.Services.Email;
public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(IOptions<EmailConfiguration> emailConfiguration)
    {
        _emailConfiguration = emailConfiguration.Value;
    }

    public async Task SendEmailAsync<T>(EmailRequestDto<T> emailRequestDto)
    {
        var template = LoadEmailTemplate(emailRequestDto.TemplateName);
        var body = ReplacePlaceholders(template, emailRequestDto.Data is not null ? emailRequestDto.Data : string.Empty);

        using (var client = new SmtpClient(_emailConfiguration.Server, _emailConfiguration.Port))
        {
            client.UseDefaultCredentials = _emailConfiguration.UseDefaultCredentials;
            client.Credentials = new NetworkCredential(_emailConfiguration.Username, _emailConfiguration.Password);
            client.EnableSsl = _emailConfiguration.EnableSsl;

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_emailConfiguration.From, _emailConfiguration.BrandName);
                message.To.Add(emailRequestDto.To);
                message.Subject = emailRequestDto.Subject;
                message.Body = body;
                message.IsBodyHtml = true;

                await client.SendMailAsync(message);
            }
        }
    }

    public string GetOTPEmailTemplateBasedOnUseCase(string useCaseName)
    {
        switch (useCaseName)
        {
            case OtpUseCases.REGISTER_OTP:
                return EmailTemplates.REGISTERION_EMAIL_TEMPLATE;
            case OtpUseCases.LOGIN_OTP:
                return EmailTemplates.LOGIN_EMAIL_TEMPLATE;
            case OtpUseCases.FORGOT_OTP:
                return EmailTemplates.FORGOT_PASSWORD_EMAIL_TEMPLATE;
            case OtpUseCases.VERIFY_EMAIL_OTP:
                return EmailTemplates.VERIFY_EMAIL_TEMPLATE;
        }

        return string.Empty;
    }

    #region Private Methods

    private string LoadEmailTemplate(string templateName)
    {
        var templatePath = Path.Combine(_emailConfiguration.TemplateDirectory, templateName + FileExtensions.HTML);
        var template = File.ReadAllText(templatePath);
        return template;
    }

    private string ReplacePlaceholders(string template, object data)
    {
        var properties = data.GetType().GetProperties();
        foreach (var property in properties)
        {
            var placeholder = $"{Characters.LEFT_BRACE}{property.Name}{Characters.RIGHT_BRACE}";
            var value = property.GetValue(data)?.ToString() ?? string.Empty;
            template = template.Replace(placeholder, value);
        }
        return template;
    }

    #endregion
}

