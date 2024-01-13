using Breeze.Models.Dtos.Token.Request;

namespace Breeze.Services.Token;

public interface ITokenService
{
    string GenerateToken(CreateTokenRequestDto requestDto);
}