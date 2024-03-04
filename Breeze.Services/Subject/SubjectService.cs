using Breeze.DbCore.UnitOfWork;
using Breeze.DBCore.Dapper;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Subject.SP;
using System.Data;

namespace Breeze.Services.Subject;
public class SubjectService : ISubjectService
{
    private readonly IDapperRepository _dapperRepository;

    public SubjectService(
        IDapperRepository dapperRepository)
    {
        _dapperRepository = dapperRepository;
    }

    public async Task<IEnumerable<SubjectSummarySPDto>> GetSubjectSummary(int subjectId)
    {
        var parameters = _dapperRepository.BuildDynamicParameters(new Dictionary<string, (int? Value, DbType Type)>
        {
            { DapperSPParams.SUBJECT_ID, (subjectId, DbType.Int32) } 
        });

        return await _dapperRepository.DapperSpListWithParamsAsync<SubjectSummarySPDto>($"{CommandConstants.EXEC_COMMAND} {StoreProcedureNames.GET_SUBJECT_SUMMARY} {DapperSPParams.STUDENT_ID}, {DapperSPParams.DISCIPLINE_ID}, {DapperSPParams.SUBJECT_ID}", parameters);
    }
}
