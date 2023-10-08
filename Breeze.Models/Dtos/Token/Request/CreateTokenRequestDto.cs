namespace Breeze.Models.Dtos.Token.Request;
public class CreateTokenRequestDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public int DisciplineId { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}