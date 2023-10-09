using Breeze.Models.Dtos.Token.Request;

namespace Breeze.Services.TokenService;

public interface ITokenService
{
    string GenerateToken(CreateTokenRequestDto requestDto);
}