using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Contexts;

public class DapperDbContext(IOptions<DapperOptions> options) : IDisposable
{
    private IDbConnection? _connection;
    private bool _disposed;
    private readonly DapperOptions _options = options.Value;

    public IDbConnection NpgConnection
    {
        get
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_options.ConnectionString);
                _connection.Open();
            }
            else if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection; 
        }
    }
    public void Dispose()
    {
        if (_disposed)
            return;

        _connection?.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
