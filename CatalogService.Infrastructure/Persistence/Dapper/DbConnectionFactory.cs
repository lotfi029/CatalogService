using CatalogService.Application;
using Microsoft.Extensions.Options;
using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper;

internal sealed class DbConnectionFactory(IOptions<DapperOptions> options) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var context = new DapperDbContext(options);
        return context.NpgConnection;
    }
}
