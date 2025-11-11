using CatalogService.Infrastructure;

namespace CatalogService.API;

public static class DependancyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddAntherLayers(configuration);

        return services;
    }

    private static IServiceCollection AddAntherLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }
}
