using Breeze.Services.DropDown;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;

public class DropDownController(IDropDownService _dropDownService) : BaseApiController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBoardDetailsDropDown()
        => Ok(await _dropDownService.GetBoardDetailsDropDown());

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCollegesDropDown()
     => Ok(await _dropDownService.GetCollegesDropDown());
}
