using Breeze.API.Extensions;
using System.Net;

namespace Breeze.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment() || _env.IsQA() ?
                GenericResponse<string>.Failure(ex.InnerException != null ? ex.InnerException.ToString() : string.Empty, ex.Message, (int)HttpStatusCode.InternalServerError) :
                GenericResponse<string>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, (int)HttpStatusCode.InternalServerError);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(response, settings);

            await context.Response.WriteAsync(json);

        }
    }
}
