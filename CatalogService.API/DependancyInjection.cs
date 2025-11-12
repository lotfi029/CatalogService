using CatalogService.API.Extensions;
using CatalogService.API.Infrastructure;
using CatalogService.Infrastructure;

namespace CatalogService.API;

public static class DependancyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi(options => 
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>()
        );
        services.AddAntherLayers(configuration);
        services.AddEndpoints(typeof(DependancyInjection).Assembly);

        return services;
    }

    private static IServiceCollection AddAntherLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructure(configuration);

        return services;
    }
}
