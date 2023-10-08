using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.OTP.Response;
public class OTPResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string OTPCode { get; set; } = string.Empty;
    public string OTPUseCase { get; set; } = string.Empty;   
}
