using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure.Persistence;

public static class ConnectionStringConstants
{
    public const string DefaultConnection = "DefaultConnection";
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
        => configuration.GetConnectionString(DefaultConnection)
        ?? throw new InvalidOperationException($"Connection string {DefaultConnection} not found.");
}
