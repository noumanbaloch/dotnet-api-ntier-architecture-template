using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Subject.SP;
using Breeze.Services.ParamBuilder;

namespace Breeze.Services.Subject;
public class SubjectService(IUnitOfWork _unitOfWork,
    IParamBuilderService _paramBuilderService) : ISubjectService
{
    public async Task<IEnumerable<SubjectSummarySPDto>> GetSubjectSummary(int subjectId)
    {
        var parameters = _paramBuilderService.BuildDynamicParameters(subjectId);
        return await _unitOfWork.DapperSpListWithParamsAsync<SubjectSummarySPDto>($"{CommandConstants.EXEC_COMMAND} {StoreProcedureNames.GET_SUBJECT_SUMMARY} {DapperSPParams.STUDENT_ID}, {DapperSPParams.DISCIPLINE_ID}, {DapperSPParams.SUBJECT_ID}", parameters);
    }
}
