using Breeze.Models.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Breeze.Services.ClaimResolver;
public class ClaimResolverService : IClaimResolverService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimResolverService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetLoggedInUsername()
        => GetClaimValue(JWTClaimNames.USER_NAME);

    public int GetUserId()
        => Convert.ToInt32(GetClaimValue(JWTClaimNames.USER_ID));

    public bool IsUserAuthenticated()
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    #region Private Methods
    private string GetClaimValue(string claimType)
    {
        if (!IsUserAuthenticated())
        {
            throw new UnauthorizedAccessException(ExceptionMessages.UNAUTHORIZED_USER);
        }

        return _httpContextAccessor.HttpContext!.User.FindFirstValue(claimType)!;
    }
    #endregion
}


