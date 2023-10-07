using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthFacadeService _authService;

    public AuthController(IAuthFacadeService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        => Ok(await _authService.Register(requestDto));

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto)
        => Ok(await _authService.Login(requestDto));

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto requestDto)
        => Ok(await _authService.ChangePassword(requestDto));

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
        => Ok(await _authService.ForgotPassword(requestDto));

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CheckForUserExists(string userName)
        => Ok(await _authService.CheckForUserExists(userName));

    [HttpPost]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDto requestDto)
        => Ok(await _authService.UpdateProfile(requestDto));
}
