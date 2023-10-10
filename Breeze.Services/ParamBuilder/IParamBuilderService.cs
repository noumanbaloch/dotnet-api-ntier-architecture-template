using Dapper;

namespace Breeze.Services.ParamBuilder;

public interface IParamBuilderService
{
    DynamicParameters BuildDynamicParameters(int? subjectId = null, int? chapterId = null, int? cardTypeId = null);
}