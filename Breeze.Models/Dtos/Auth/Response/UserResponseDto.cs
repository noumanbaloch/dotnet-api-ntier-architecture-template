namespace Breeze.Models.Dtos.Auth.Response;
public class UserResponseDto
{
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Token { get; set; }
    public required string PhotoUrl { get; set; }
}