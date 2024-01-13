using Breeze.DbCore.GenericRepository;
using Dapper;
using System.Data;

namespace Breeze.DbCore.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    Task<IEnumerable<TEntity>> DapperSpListWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<IEnumerable<TEntity>> DapperSpListWithoutParamsAsync<TEntity>(string spName);
    Task<TEntity?> DapperSpSingleWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<TEntity?> DapperSpSingleWithoutParamsAsync<TEntity>(string spName);
    Task DapperSpExecuteWithParamsAsync(string spName, DynamicParameters parameters);
    Task DapperSpExecuteWithoutParamsAsync(string spName);
    DynamicParameters BuildDynamicParameters<T>(Dictionary<string, (T Value, DbType Type)>? parametersDictionary = default);
    Task<int> CommitAsync();
}