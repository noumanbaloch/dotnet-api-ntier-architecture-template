using Breeze.DBCore.Factory;
using Breeze.Models.Configurations;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data;

namespace Breeze.DBCore.Dapper;
public class DapperRepository : IDapperRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    private readonly DatabaseConfiguration _databaseConfiguration;

    public DapperRepository(ISqlConnectionFactory connectionFactory,
         IOptions<DatabaseConfiguration> databaseConfiguration)
    {
        _connectionFactory = connectionFactory;
        _databaseConfiguration = databaseConfiguration.Value;
    }

    public IEnumerable<TEntity> DapperSpListWithParams<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout).ToList();
    }

    public IEnumerable<TEntity> DapperSpListWithoutParams<TEntity>(string spName)
    {
        using var connection = _connectionFactory.CreateConnection();
        return connection.Query<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout).ToList();
    }

    public async Task<IEnumerable<TEntity>> DapperSpListWithParamsAsync<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = _connectionFactory.CreateConnection();
        return (await connection.QueryAsync<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout)).ToList();
    }

    public async Task<IEnumerable<TEntity>> DapperSpListWithoutParamsAsync<TEntity>(string spName)
    {
        using var connection = _connectionFactory.CreateConnection();
        return (await connection.QueryAsync<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout)).ToList();
    }

    public TEntity? DapperSpSingleWithParams<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public TEntity? DapperSpSingleWithoutParams<TEntity>(string spName)
    {
        using var connection = _connectionFactory.CreateConnection();
        return connection.QueryFirstOrDefault<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task<TEntity?> DapperSpSingleWithParamsAsync<TEntity>(string spName, DynamicParameters parameters)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TEntity>(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task<TEntity?> DapperSpSingleWithoutParamsAsync<TEntity>(string spName)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TEntity>(spName, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task DapperSpExecuteWithoutParamsAsync(string spName)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(spName, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public async Task DapperSpExecuteWithParamsAsync(string spName, DynamicParameters parameters)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(spName, parameters, commandTimeout: _databaseConfiguration.CommandTimeout);
    }

    public DynamicParameters BuildDynamicParameters<T>(Dictionary<string, (T Value, DbType Type)>? parametersDictionary = default)
    {
        DynamicParameters parameters = new();

        if (parametersDictionary != null)
        {
            foreach (var kvp in parametersDictionary)
            {
                parameters.Add(kvp.Key, kvp.Value.Value, kvp.Value.Type, direction: ParameterDirection.Input);
            }
        }

        return parameters;
    }
}
