using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Subject.SP;
using Breeze.Services.ParamBuilder;

namespace Breeze.Services.Subject;
public class SubjectService : ISubjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IParamBuilderService _paramBuilderService;
    public SubjectService(IUnitOfWork unitOfWork,
        IParamBuilderService paramBuilderService)
    {
        _unitOfWork = unitOfWork;
        _paramBuilderService = paramBuilderService;
    }

    public async Task<IEnumerable<SubjectSummarySPDto>> GetSubjectsSummary(int subjectId)
    {
        var parameters = _paramBuilderService.BuildDynamicParameters(subjectId);
        return await _unitOfWork.DapperSpListWithParamsAsync<SubjectSummarySPDto>($"{CommandConstants.EXEC_COMMAND} {StoreProcedureNames.GET_SUBJECTS_SUMMARY} {DapperSPParams.STUDENT_ID}, {DapperSPParams.DISCIPLINE_ID}, {DapperSPParams.SUBJECT_ID}", parameters);
    }
}
