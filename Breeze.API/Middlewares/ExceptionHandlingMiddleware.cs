using Breeze.API.Extensions;
using Breeze.Models.Constants;
using Breeze.Models.GenericResponses;
using Breeze.Services.Logging;
using System.Net;
using System.Text.Json;

namespace Breeze.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context,
        ILoggingService loggingService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await loggingService.LogException(ex);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var payload = ex.InnerException != null ? ex.InnerException.ToString() : string.Empty;
            var response = _env.IsDevelopment() || _env.IsQA() ?
                GenericResponse<string>.Failure(payload, ex.Message, (int)HttpStatusCode.InternalServerError) :
                GenericResponse<string>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, (int)HttpStatusCode.InternalServerError);

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
