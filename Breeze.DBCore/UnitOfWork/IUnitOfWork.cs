using Breeze.DbCore.GenericRepository;
using Dapper;

namespace Breeze.DbCore.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<T> GetRepository<T>() where T : class;
    IEnumerable<TEntity> DapperSpListWithParams<TEntity>(string spName, DynamicParameters parameters);
    IEnumerable<TEntity> DapperSpListWithoutParams<TEntity>(string spName);
    Task<IEnumerable<TEntity>> DapperSpListWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<IEnumerable<TEntity>> DapperSpListWithoutParamsAsync<TEntity>(string spName);
    TEntity DapperSpSingleWithParams<TEntity>(string spName, DynamicParameters parameters);
    TEntity DapperSpSingleWithoutParams<TEntity>(string spName);
    Task<TEntity> DapperSpSingleWithParamsAsync<TEntity>(string spName, DynamicParameters parameters);
    Task<TEntity> DapperSpSingleWithoutParamsAsync<TEntity>(string spName);
    int Commit();
    Task<int> CommitAsync();
}