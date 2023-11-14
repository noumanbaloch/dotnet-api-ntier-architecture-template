using Microsoft.AspNetCore.Http;

namespace Breeze.Services.HttpHeader;
public class HttpHeaderService(IHttpContextAccessor _httpContextAccessor) : IHttpHeaderService
{

    public string GetHeader(string headerName)
        => _httpContextAccessor?.HttpContext?.Request.Headers.TryGetValue(headerName, out var headerValue) == true ? 
        headerValue.ToString() :
        string.Empty;
}
