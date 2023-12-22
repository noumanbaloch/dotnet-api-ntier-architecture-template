using Breeze.Models.Configurations;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Token.Request;
using Breeze.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Breeze.Services.TokenService;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly AuthenticationConfiguration _authenticationConfiguration;

    public TokenService(IOptions<AuthenticationConfiguration> authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.SecretKey));
    }

    public string GenerateToken(CreateTokenRequestDto requestDto)
    {
        var claims = new List<Claim>()
            {
                new (JwtClaimNames.USER_ID, requestDto.Id.ToString()),
                new (JwtClaimNames.USER_NAME, requestDto.UserName ?? string.Empty),
                new (JwtClaimNames.FIRST_NAME, requestDto.FirstName ?? string.Empty),
                new (JwtClaimNames.LAST_NAME, requestDto.LastName ?? string.Empty),
                new (JwtClaimNames.FULL_NAME, $"{requestDto.FirstName} {requestDto.LastName}"),
                new (JwtClaimNames.PHONE_NUMBER, requestDto.PhoneNumber ?? string.Empty),
                new (JwtClaimNames.JTI, Guid.NewGuid().ToString()),
                new (JwtClaimNames.IAT, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
            };

        foreach (var role in requestDto.Roles)
        {
            claims.Add(new Claim(type: ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_key, algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                   issuer: _authenticationConfiguration.Issuer,
                   audience: _authenticationConfiguration.Audience,
                   claims: claims,
                   expires: Helper.GetCurrentDate().AddDays(MagicNumbers.TOKEN_EXPIRY_DAYS),
                   signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);

    }

}