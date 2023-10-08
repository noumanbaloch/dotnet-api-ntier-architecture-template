using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Dtos.Token.Request;
using Breeze.Models.Entities;

namespace Breeze.Models.ModelMapping;

public static class EntityToDtoMappingExtensions
{
    public static UserResponseDto ToUserResponseDto(this UserEntity user, string token)
    {
        return new UserResponseDto
        {
            UserName = user.UserName!,
            Token = token,
            PhotoUrl = string.Empty
        };
    }

    public static CreateTokenRequestDto ToCreateTokenRequesDto(this UserEntity user, List<string> roles, StudentEntity student)
    {
        return new CreateTokenRequestDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            StudentId = student.Id,
            DisciplineId = student.DisciplineId,
            Roles = roles
        };
    }
}