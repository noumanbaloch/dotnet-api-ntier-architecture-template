using Breeze.DbCore.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Breeze.DbCore.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly IDatabaseContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(IDatabaseContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void AddList(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public async Task AddListAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(TEntity entity)
    {
        if (Helper.IsNullOrEmpty(entity))
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var entry = _dbSet.Update(entity);

        // Retrieve the property
        var rowVersionProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == PropertyNames.ROW_VERSION);

        if (rowVersionProperty is not null)
        {
            try
            {
                long currentValue = BitConverter.ToInt64((byte[])rowVersionProperty.OriginalValue!, 0);
                long newValue = BitConverter.ToInt64((byte[])rowVersionProperty.CurrentValue!, 0);

                if (rowVersionProperty.IsModified && currentValue != newValue)
                {
                    throw new DbUpdateConcurrencyException(ExceptionMessages.RECORD_MODIFIED_BY_OTHER_USER);
                }

                rowVersionProperty.CurrentValue = BitConverter.GetBytes(newValue + 1);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionMessages.ERROR_UPDATING_ROW_VERSION, ex);
            }
        }
    }

    public void UpdateList(IEnumerable<TEntity> entites)
    {
        foreach (var entitiesChunk in entites.Chunk(10))
        {
            _dbSet.UpdateRange(entitiesChunk);
        }
    }

    public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate).ToList();
    }

    public IEnumerable<TEntity> FindByNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.AsNoTracking().Where(predicate).ToList();
    }

    public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindByNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public IEnumerable<TEntity> GetAll()
    {
        return _dbSet.ToList();
    }

    public IEnumerable<TEntity> GetAllNoTracking()
    {
        return _dbSet.AsNoTracking().ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllNoTrackingAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public TEntity? GetById<TKey>(TKey id)
    {
        return _dbSet.Find(id);
    }

    public async Task<TEntity?> GetByIdAsync<TKey>(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public TEntity? FindByFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate).FirstOrDefault();
    }

    public TEntity? FindByFirstOrDefaultNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.AsNoTracking().Where(predicate).FirstOrDefault();
    }

    public async Task<TEntity?> FindByFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).FirstOrDefaultAsync()!;
    }

    public async Task<TEntity?> FindByFirstOrDefaultNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
    }

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.AsNoTracking().Where(predicate).Any();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).AnyAsync();
    }
}