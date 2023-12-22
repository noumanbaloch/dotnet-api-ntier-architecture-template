using Breeze.Models.Entities;

namespace Breeze.Identity;

public interface IIdentityService
{
    Task<bool> CreateUser(UserEntity user, string password);
    Task<bool> AddUserRole(UserEntity user, string role);
    Task DeleteUser(UserEntity user);
    Task<IList<string>> GetUserRoles(UserEntity user);
    Task<UserEntity?> FindByUserName(string userName);
    Task<bool> ValidateUserPassword(UserEntity user, string currentPassword);
    Task<bool> ChangeUserPassword(UserEntity user, string newPassword);
    Task UpdateUser(UserEntity user);
}
