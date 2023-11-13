using Breeze.Models.Constants;
using Breeze.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Breeze.API.Filter;

public class DeviceValidatorAttribute : ActionFilterAttribute
{
    private readonly IAuthService _authService;

    public DeviceValidatorAttribute(IAuthService authService)
    {
        _authService = authService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        var username = context.HttpContext.User.FindFirstValue(JwtClaimNames.USER_NAME);

        if (!isAnonymous && username is not null && !await _authService.ValidateTrustedDevice(username, context.HttpContext.Request.Headers[PropertyNames.DEVICE_ID]!))
        {
            context!.Result = new UnauthorizedObjectResult(ApiResponseMessages.UN_RECOGNIZED_DEVICE);
            return;
        }
        await next();
    }
}