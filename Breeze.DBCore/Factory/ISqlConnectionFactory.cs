using System.Data;

namespace Breeze.DBCore.Factory;
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
