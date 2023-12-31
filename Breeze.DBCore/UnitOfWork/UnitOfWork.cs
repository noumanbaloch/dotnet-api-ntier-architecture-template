﻿using Breeze.DbCore.Context;
using Breeze.DbCore.GenericRepository;
using Breeze.Models.Configurations;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Breeze.DbCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IBreezeDbContext? _dbContext;
    private readonly string _connectionString;
    private bool disposed = false;
    private readonly DatabaseConfiguration _databaseConfiguration;
    private Dictionary<Type, object> _repos = [];

    public UnitOfWork(IBreezeDbContext? dbContext,
        BreezeDbContext dbConnectionContext,
        IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        _dbContext = dbContext;
        _connectionString = dbConnectionContext.Database.GetConnectionString()!;
        _databaseConfiguration = databaseConfiguration.Value;
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

    public async Task<IEnumerable<TEntity>> DapperSpListWithParamsAsync<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        return (await connection.QueryAsync<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout)).ToList();
    }

    public async Task<IEnumerable<TEntity>> DapperSpListWithoutParamsAsync<TEntity>(string spName)
    {
        using var connection = new SqlConnection(_connectionString);
        return (await connection.QueryAsync<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout)).ToList();
    }

    public async Task<TEntity?> DapperSpSingleWithParamsAsync<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task<TEntity?> DapperSpSingleWithoutParamsAsync<TEntity>(string spName)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task DapperSpExecuteWithoutParamsAsync(string spName)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(spName, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task DapperSpExecuteWithParamsAsync(string spName, DynamicParameters parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout);
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
