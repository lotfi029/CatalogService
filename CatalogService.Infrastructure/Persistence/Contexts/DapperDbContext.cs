using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Contexts;

public class DapperDbContext(IConfiguration configuration) : IDisposable
{
    private readonly string _connectionString
        = configuration.GetConnectionStringOrThrow(ConnectionStringConstants.DefaultConnection);

    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    public IDbConnection? Connection
    {
        get
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_connectionString);
                _connection.Open();
            }
            else if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection; 
        }
    }

    public IDbTransaction? Transaction => _transaction;

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }
    public void RollBack()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _transaction?.Rollback();
        _transaction?.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}
