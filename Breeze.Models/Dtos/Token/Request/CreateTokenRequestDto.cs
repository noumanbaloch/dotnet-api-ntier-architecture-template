namespace Breeze.Models.Dtos.Token.Request;
public class CreateTokenRequestDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public int StudentId { get; set; }
    public int DisciplineId { get; set; }
    public required List<string> Roles { get; set; }
}