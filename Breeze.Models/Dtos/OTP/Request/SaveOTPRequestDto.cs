namespace Breeze.Models.Dtos.OTP.Request;
public class SaveOTPRequestDto
{
    public required string UserName { get; set; }
    public required string OTPCode { get; set; }
    public required string OTPUseCase { get; set; }
}
