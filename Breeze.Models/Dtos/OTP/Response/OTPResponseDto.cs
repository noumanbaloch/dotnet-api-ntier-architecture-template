using System.ComponentModel.DataAnnotations;

namespace Breeze.Models.Dtos.OTP.Response;
public class OTPResponseDto
{
    public required string UserName { get; set; }
    public required string OTPCode { get; set; }
    public required string OTPUseCase { get; set; }
}
