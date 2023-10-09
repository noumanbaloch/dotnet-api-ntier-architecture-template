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

    public string? GetLoggedInUsername()
        => IsUserAuthenticated()
            ? _httpContextAccessor.HttpContext?.User.FindFirstValue(JWTClaimNames.USER_NAME)
            : null;

    public int? GetUserId()
      => IsUserAuthenticated()
          && int.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(JWTClaimNames.USER_ID), out int userId)
          ? userId
          : null;

    private bool IsUserAuthenticated()
        => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
}


