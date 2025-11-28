namespace CatalogService.Application;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
