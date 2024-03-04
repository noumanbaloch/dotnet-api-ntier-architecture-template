using Breeze.DbCore.Context;
using Breeze.DbCore.GenericRepository;
using Breeze.Models.Configurations;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace Breeze.DbCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IBreezeDbContext? _dbContext;
    private bool disposed = false;
    private Dictionary<Type, object> _repos = [];

    public UnitOfWork(IBreezeDbContext? dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        _repos ??= [];

        var type = typeof(TEntity);
        if (!_repos.ContainsKey(type) && _dbContext is not null)
        {
            IGenericRepository<TEntity> repo = new GenericRepository<TEntity>(_dbContext);
            _repos.Add(type, repo);
        }

        return (IGenericRepository<TEntity>)_repos[type];
    }

    public async Task<int> CommitAsync()
    {
        if (_dbContext is null) return 0;
        return await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing && _dbContext is not null)
        {
            _dbContext.Dispose();
            _dbContext = null;
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
