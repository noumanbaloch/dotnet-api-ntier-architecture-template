using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Entities;
using Breeze.Services.ClaimResolver;
using Breeze.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Breeze.Services.Logging;
public class LoggingService : ILoggingService 
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClaimResolverService _claimResolverService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoggingService> _logger;
    public LoggingService(IHttpContextAccessor httpContextAccessor,
        IClaimResolverService claimResolverService,
        IUnitOfWork unitOfWork,
        ILogger<LoggingService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _claimResolverService = claimResolverService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task LogException(Exception ex)
    {
        LogToAzureBlogStorage(ex);
        await LogToDatabase(ex);
    }

    #region Private Methods

    private async Task LogToDatabase(Exception ex)
    {
        var entity = new LogEntryErrorEntity()
        {
            Exception = ex.InnerException?.ToString() ?? ex.ToString(),
            Message = ex.Message,
            RequestHeaders = GetRequestHeaders(),
            RequestMethod = GetRequestMethod(),
            RequestPath = GetRequestPath(),
            StackTrace = ex.StackTrace ?? string.Empty,
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Source = ex.Source ?? string.Empty,
            UserDescription = _claimResolverService.IsUserAuthenticated() ? $"UserId: {_claimResolverService.GetUserId()} - Username: {_claimResolverService.GetLoggedInUsername()}" : null,
            CreatedBy = Usernames.SYSTEM_USERNAME,
            CreatedDate = DateTime.Now,

        };

        var repo = _unitOfWork.GetRepository<LogEntryErrorEntity>();

        await repo.AddAsync(entity);

        await _unitOfWork.CommitAsync();

    }
    private void LogToAzureBlogStorage(Exception ex)
        => _logger.LogError(ex, message: ex.Message);

    private string GetRequestHeaders()
        => Newtonsoft.Json.JsonConvert.SerializeObject(_httpContextAccessor.HttpContext!.Request.Headers);

    private string GetRequestMethod()
        => _httpContextAccessor.HttpContext!.Request.Method;

    private string GetRequestPath()
        => _httpContextAccessor.HttpContext!.Request.GetDisplayUrl();
    #endregion
}
