using AutoMapper;
using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Entities;
using Breeze.Services.Cache;
using Breeze.Services.HttpHeader;
using Breeze.Utilities;

namespace Breeze.Services.Auth;
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpHeaderService _httpHeaderService;
    private readonly ICacheService _cacheService;

    public AuthService(IUnitOfWork unitOfWork,
        IHttpHeaderService httpHeaderService,
        IMapper mapper,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _httpHeaderService = httpHeaderService;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task UpdateDevice(string userName)
    {
        var repo = _unitOfWork.GetRepository<UserEntity>();
        var entity = await repo.FindByFirstOrDefaultAsync(x => x.UserName!.ToLower() == userName.ToLower()
        && x.Deleted == false);

        var newDeviceId = _httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString();
        if (entity is not null)
        {
            _cacheService.RemoveData($"{CacheKeys.TRUSTED_DEVICE}{userName}{entity.TrustedDeviceId}");
            _cacheService.SetData($"{CacheKeys.TRUSTED_DEVICE}{userName}{newDeviceId}", newDeviceId);

            entity.TrustedDeviceId = newDeviceId;
            entity.ModifiedBy = userName;
            entity.ModifiedDate = Helper.GetCurrentDate();
            repo.Update(entity);
        }

        await _unitOfWork.CommitAsync();
    }
    public async Task<bool> UserExists(string userName) => await _unitOfWork.GetRepository<UserEntity>()
             .AnyAsync(x => x.UserName!.ToLower() == userName.ToLower()
             && x.Deleted == false);

    public async Task<UserEntity?> GetUserByUsername(string username)
    {
        return await _unitOfWork.GetRepository<UserEntity>().FindByFirstOrDefaultAsync(x => x.UserName!.ToLower() == username.ToLower()
        && x.Deleted == false);
    }

    public async Task<UserEntity?> GetUserByUserId(int userId)
    {
        return await _unitOfWork.GetRepository<UserEntity>().FindByFirstOrDefaultAsync(x => x.Id == userId
        && x.Deleted == false);
    }

    public async Task<bool> ValidateTrustedDevice(string userName, string deviceId)
    {
        var cacheKey = $"{CacheKeys.TRUSTED_DEVICE}{userName}{deviceId}";
        var cacheTrusted = _cacheService.GetData<string>(cacheKey);
        if (string.IsNullOrWhiteSpace(cacheTrusted))
        {
            var user = await GetUserByUsername(userName);
            if (user is null || user.TrustedDeviceId != deviceId)
            {
                return false;
            }

            if (_cacheService.Exists(cacheKey) == false)
            {
                _cacheService.SetData(cacheKey, user.TrustedDeviceId);
            }
            return true;
        }
        return true;
    }

    public async Task<bool> UserhandleAlreadyExist(string userHandle, string userName)
        => await _unitOfWork.GetRepository<UserEntity>().AnyAsync(x => x.UserHandle == userHandle &&
        x.UserName != userName &&
        x.Deleted == false);
}
