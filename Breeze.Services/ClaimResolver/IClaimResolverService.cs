namespace Breeze.Services.ClaimResolver;
public interface IClaimResolverService
{
    string GetLoggedInUsername();
    int GetUserId();
    bool IsUserAuthenticated();
}
