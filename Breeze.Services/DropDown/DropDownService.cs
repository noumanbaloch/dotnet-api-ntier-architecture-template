using AutoMapper;
using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.DropDown.Response;
using Breeze.Models.Entities;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.DropDown;
public class DropDownService : IDropDownService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DropDownService(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenericResponse<IEnumerable<DropDownResponseDto>>> GetBoardDetailsDropDown()
    {
        var dropDownResponseDto = await GetDropDown<BoardDetailEntity>();

        if (dropDownResponseDto is not null)
        {
            return GenericResponse<IEnumerable<DropDownResponseDto>>.Success(dropDownResponseDto, ApiResponseMessages.RECORD_FOUND, ApiStatusCodes.RECORD_FOUND);
        }
        return GenericResponse<IEnumerable<DropDownResponseDto>>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
    }

    public async Task<GenericResponse<IEnumerable<DropDownResponseDto>>> GetCollegesDropDown()
    {
        var dropDownResponseDto = await GetDropDown<CollegeEntity>();

        if (dropDownResponseDto is not null)
        {
            return GenericResponse<IEnumerable<DropDownResponseDto>>.Success(dropDownResponseDto, ApiResponseMessages.RECORD_FOUND, ApiStatusCodes.RECORD_FOUND);
        }
        return GenericResponse<IEnumerable<DropDownResponseDto>>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
    }

    private async Task<IEnumerable<DropDownResponseDto>?> GetDropDown<TEntity>() where TEntity : BaseEntity
    {
        var result = await _unitOfWork.GetRepository<TEntity>()
            .GetAllNoTrackingAsync();

        var dropDownResponseDto = _mapper.Map<IEnumerable<DropDownResponseDto>>(result);

        if (dropDownResponseDto is not null && dropDownResponseDto.Any())
        {
            return dropDownResponseDto;
        }

        return default;
    }

}
