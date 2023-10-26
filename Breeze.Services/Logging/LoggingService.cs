using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Entities;
using Breeze.Services.ClaimResolver;
using Breeze.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace Breeze.Services.Logging;
public class LoggingService : ILoggingService 
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClaimResolverService _claimResolverService;
    private readonly IUnitOfWork _unitOfWork;
    public LoggingService(IHttpContextAccessor httpContextAccessor,
        IClaimResolverService claimResolverService,
        IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _claimResolverService = claimResolverService;
        _unitOfWork = unitOfWork;
    }
    public async Task LogException(Exception ex)
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
            UserDescription = @$"{_claimResolverService.GetUserId()}",
            CreatedBy = !string.IsNullOrWhiteSpace(_claimResolverService.GetLoggedInUsername()) ? _claimResolverService.GetLoggedInUsername()! : Usernames.SYSTEM_USERNAME,
            CreatedDate = Helper.GetCurrentDate()
        };

        var repo = _unitOfWork.GetRepository<LogEntryErrorEntity>();

        repo.Add(entity);

        await _unitOfWork.CommitAsync();
    }

    #region Private Methods
    private string GetRequestHeaders()
        => Newtonsoft.Json.JsonConvert.SerializeObject(_httpContextAccessor.HttpContext.Request.Headers);

    private string GetRequestMethod()
        => _httpContextAccessor.HttpContext.Request.Method;

    private string GetRequestPath()
        => _httpContextAccessor.HttpContext.Request.GetDisplayUrl();

    #endregion
}
