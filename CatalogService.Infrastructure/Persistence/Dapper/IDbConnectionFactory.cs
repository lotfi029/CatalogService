using System.Data;

namespace CatalogService.Infrastructure.Persistence.Dapper;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
