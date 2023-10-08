namespace Breeze.Models.Dtos.Auth.Response;
public class UserResponseDto
{
    public string UserName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;   
    public string PhotoUrl { get; set; } = string.Empty;
}