using Breeze.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Breeze.Identity;
public class IdentityService : IIdentityService
{
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;
    public IdentityService(SignInManager<UserEntity> signInManager,
        UserManager<UserEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<bool> CreateUser(UserEntity user, string password)
        => (await _userManager.CreateAsync(user, password)).Succeeded;

    public async Task<bool> AddUserRole(UserEntity user, string role)
        => (await _userManager.AddToRoleAsync(user, role)).Succeeded;

    public async Task DeleteUser(UserEntity user)
        => await _userManager.DeleteAsync(user);

    public async Task<IList<string>> GetUserRoles(UserEntity user)
        => await _userManager.GetRolesAsync(user);

    public async Task<UserEntity?> FindByUserName(string userName)
        => await _userManager.FindByNameAsync(userName);

    public async Task<bool> ValidateUserPassword(UserEntity user, string currentPassword)
        => (await _signInManager.CheckPasswordSignInAsync(user, currentPassword, false)).Succeeded;

    public async Task<bool> ChangeUserPassword(UserEntity user, string newPassword)
    {
        await _signInManager.SignOutAsync();
        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        return (await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword)).Succeeded;
    }

    public async Task UpdateUser(UserEntity user)
        => await _userManager.UpdateAsync(user);
}
