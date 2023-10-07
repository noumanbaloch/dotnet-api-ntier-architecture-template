using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Breeze.API.Controllers;

public class DropDownController : BaseApiController
{
    private readonly IDropDownService _dropDownService;

    public DropDownController(IDropDownService dropDownService)
    {
        _dropDownService = dropDownService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetBoardDetailsDropDown()
        => Ok(await _dropDownService.GetBoardDetailsDropDown());

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCollegesDropDown()
     => Ok(await _dropDownService.GetCollegesDropDown());
}
