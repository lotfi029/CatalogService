using Microsoft.Extensions.Configuration;

namespace CatalogService.Infrastructure.Persistence.ConnectionStrings;

public static class ConnectionStringBuilder
{
    public const string DefaultConnection = "DefaultConnection";
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string? name = null)
        => configuration.GetConnectionString(name ?? DefaultConnection)
        ?? throw new InvalidOperationException($"Connection string {DefaultConnection} not found.");
}