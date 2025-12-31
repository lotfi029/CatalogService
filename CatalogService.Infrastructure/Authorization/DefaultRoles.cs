namespace CatalogService.Infrastructure.Authorization;

public sealed record DefaultRoles
{
    public static string Vendor => "vendor";
    public static string Admin => "admin";
    public static string User => "user";
}

