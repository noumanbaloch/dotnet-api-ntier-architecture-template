using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Email.Request;
public class EmailRequestDto<T>
{
    [Required]
    public string To { get; set; } = string.Empty;

    [Required]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string TemplateName { get; set; } = string.Empty;

    [Required]
    public T? Data { get; set; }
}
