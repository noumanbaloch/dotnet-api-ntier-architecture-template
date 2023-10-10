using Breeze.Models.Constants;
using Breeze.Services.ClaimResolver;
using Dapper;
using System.Data;

namespace Breeze.Services.ParamBuilder;

public class ParamBuilderService : IParamBuilderService
{
    private readonly IClaimResolverService _claimResolverService;

    public ParamBuilderService(IClaimResolverService claimResolverService)
    {
        _claimResolverService = claimResolverService;
    }

    public DynamicParameters BuildDynamicParameters(int? subjectId = null, int? chapterId = null, int? cardTypeId = null)
    {
        DynamicParameters parameters = new();
        parameters.Add(DapperSPParams.SUBJECT_ID, subjectId, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add(DapperSPParams.CHAPTER_ID, chapterId, DbType.Int32, direction: ParameterDirection.Input);
        parameters.Add(DapperSPParams.CARD_TYPE_ID, cardTypeId, DbType.Int32, direction: ParameterDirection.Input);
        return parameters;
    }
}