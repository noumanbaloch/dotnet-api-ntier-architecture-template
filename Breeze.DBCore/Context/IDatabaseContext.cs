using Microsoft.EntityFrameworkCore;

namespace Breeze.DbCore.Context;

public interface IDatabaseContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
    void Dispose();
}