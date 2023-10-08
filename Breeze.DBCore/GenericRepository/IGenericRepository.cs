using System.Linq.Expressions;

namespace Breeze.DbCore.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    Task AddAsync(TEntity entity);
    void AddList(IEnumerable<TEntity> entities);
    Task AddListAsync(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateList(IEnumerable<TEntity> entites);
    IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> FindByNoTracking(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity> GetAllNoTracking();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllNoTrackingAsync();
    TEntity? GetById<TKey>(TKey id);
    Task<TEntity?> GetByIdAsync<TKey>(TKey id);
    TEntity? FindByFirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    TEntity? FindByFirstOrDefaultNoTracking(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindByFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FindByFirstOrDefaultNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    bool Any(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}