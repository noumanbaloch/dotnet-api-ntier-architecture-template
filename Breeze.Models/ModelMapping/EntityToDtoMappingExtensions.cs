using Breeze.Models.Dtos.Auth.Response;
using Breeze.Models.Dtos.Token.Request;
using Breeze.Models.Entities;

namespace Breeze.Models.ModelMapping;

public static class EntityToDtoMappingExtensions
{
    public static UserResponseDto ToUserResponseDto(this UserEntity user, string token)
        => new ()
        {
            UserName = user.UserName!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };

    public static CreateTokenRequestDto ToCreateTokenRequesDto(this UserEntity user, IList<string> roles)
        => new ()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            Roles = roles
        };
}