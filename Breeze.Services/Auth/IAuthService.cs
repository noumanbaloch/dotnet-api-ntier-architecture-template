using Breeze.Models.Entities;

namespace Breeze.Services.Auth;
public interface IAuthService
{
    Task UpdateDevice(string userName);
    Task<bool> UserExists(string userName);
    Task<UserEntity?> GetUserByUsername(string userName);
    Task<UserEntity?> GetUserByUserId(int userId);
    Task<bool> ValidateTrustedDevice(string userName, string deviceId);
    Task<bool> UserhandleAlreadyExist(string userHandle, string userName);

}
