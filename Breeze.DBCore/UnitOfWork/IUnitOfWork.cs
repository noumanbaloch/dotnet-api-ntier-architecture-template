using Breeze.DbCore.GenericRepository;
using Dapper;

namespace Breeze.DbCore.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    Task<IEnumerable<TEntity>> DapperSpListWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<IEnumerable<TEntity>> DapperSpListWithoutParamsAsync<TEntity>(string spName);
    Task<TEntity?> DapperSpSingleWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<TEntity?> DapperSpSingleWithoutParamsAsync<TEntity>(string spName);
    Task DapperSpExecuteWithoutParamsAsync(string spName);
    Task DapperSpExecuteWithParamsAsync(string spName, DynamicParameters parameters);
    Task<int> CommitAsync();
}