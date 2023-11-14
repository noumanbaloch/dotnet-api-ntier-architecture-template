using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.Email.Request;
public class EmailRequestDto<T>
{
    [Required]
    public required string To { get; set; }

    [Required]
    public required string Subject { get; set; }

    [Required]
    public required string TemplateName { get; set; }

    [Required]
    public T? Data { get; set; }
}
