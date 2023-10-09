using Breeze.Models.Dtos.DropDown.Response;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.DropDown;

public interface IDropDownService
{
    Task<GenericResponse<IEnumerable<DropDownResponseDto>>> GetBoardDetailsDropDown();
    Task<GenericResponse<IEnumerable<DropDownResponseDto>>> GetCollegesDropDown();
}