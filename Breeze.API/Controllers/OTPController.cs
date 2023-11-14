using Breeze.Models.Dtos.OTP.Request;
using Breeze.Services.OTP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;
public class OTPController(IOTPFacadeService _OTPFacadeService) : BaseApiController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateOTP([FromBody] GenerateOTPRequestDto requestDto)
        => Ok(await _OTPFacadeService.GenerateOTP(requestDto));

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequestDto requestDto)
        => Ok(await _OTPFacadeService.VerifyOTP(requestDto));
}
