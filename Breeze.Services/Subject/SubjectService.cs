using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Subject.SP;
using System.Data;

namespace Breeze.Services.Subject;
public class SubjectService : ISubjectService
{
    private readonly IUnitOfWork _unitOfWork;
    public SubjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SubjectSummarySPDto>> GetSubjectSummary(int subjectId)
    {
        var parameters = _unitOfWork.BuildDynamicParameters(new Dictionary<string, (int? Value, DbType Type)>
        {
            { DapperSPParams.SUBJECT_ID, (subjectId, DbType.Int32) } 
        });

        return await _unitOfWork.DapperSpListWithParamsAsync<SubjectSummarySPDto>($"{CommandConstants.EXEC_COMMAND} {StoreProcedureNames.GET_SUBJECT_SUMMARY} {DapperSPParams.STUDENT_ID}, {DapperSPParams.DISCIPLINE_ID}, {DapperSPParams.SUBJECT_ID}", parameters);
    }
}
