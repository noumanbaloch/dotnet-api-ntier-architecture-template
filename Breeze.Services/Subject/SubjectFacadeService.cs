using Breeze.Models.Constants;
using Breeze.Models.Dtos.Subject.SP;
using Breeze.Models.GenericResponses;

namespace Breeze.Services.Subject;
public class SubjectFacadeService : ISubjectFacadeService
{
    private readonly ISubjectService _subjectService;

    public SubjectFacadeService(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    public async Task<GenericResponse<IEnumerable<SubjectSummarySPDto>>> GetSubjectSummary(int subjectId)
    {
        var result = await _subjectService.GetSubjectSummary(subjectId);

        if (!result.Any())
        {
            return GenericResponse<IEnumerable<SubjectSummarySPDto>>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        return GenericResponse<IEnumerable<SubjectSummarySPDto>>.Success(result, ApiResponseMessages.RECORD_FOUND, ApiStatusCodes.RECORD_FOUND);
    }
}