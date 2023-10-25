
namespace Breeze.Services.Logging;
public interface ILoggingService
{
    Task LogException(Exception ex);
}