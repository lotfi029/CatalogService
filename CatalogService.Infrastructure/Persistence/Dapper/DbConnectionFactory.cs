using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper;

internal sealed class DbConnectionFactory(IOptions<DapperOptions> options) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(options.Value.ConnectionString);
        connection.Open();
        return connection;
    }
}
