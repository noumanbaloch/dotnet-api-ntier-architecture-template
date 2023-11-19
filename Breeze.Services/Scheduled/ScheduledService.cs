
using Breeze.DbCore.UnitOfWork;

namespace Breeze.Services.Scheduled;
public class ScheduledService : IScheduledService
{
    private readonly IUnitOfWork _unitOfWork;
    public ScheduledService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;       
    }

    public async Task TestMethod()
    {
       await Task.CompletedTask;
    }
}
