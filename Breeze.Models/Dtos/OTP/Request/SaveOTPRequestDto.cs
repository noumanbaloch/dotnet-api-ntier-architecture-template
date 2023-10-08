namespace Breeze.Models.Dtos.OTP.Request;
public class SaveOTPRequestDto
{
    public string UserName { get; set; } = string.Empty;
    public string OTPCode { get; set; } = string.Empty;
    public string OTPUseCase { get; set; } = string.Empty;
}
