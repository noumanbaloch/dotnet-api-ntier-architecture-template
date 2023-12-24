using System.Linq.Expressions;

namespace Breeze.DbCore.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task AddListAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateList(IEnumerable<TEntity> entites);
    Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllNoTrackingAsync();
    Task<TEntity?> GetByIdAsync<TKey>(TKey id);
    Task<TEntity?> FindByFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindByFirstOrDefaultNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}