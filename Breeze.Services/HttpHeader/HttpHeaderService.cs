using Microsoft.AspNetCore.Http;

namespace Breeze.Services.HttpHeader;
public class HttpHeaderService : IHttpHeaderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpHeaderService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetHeader(string headerName)
    {
        if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(headerName, out var headerValue))
        {
            return headerValue.ToString();
        }
        else
        {
            return string.Empty;
        }
    }
}
