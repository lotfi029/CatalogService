using CatalogService.Domain.DomainService;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Domain;

public static class DependancyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddDomainService();

        return services;
    }

    private static IServiceCollection AddDomainService(this IServiceCollection services)
    {
        services.AddScoped<ICategoryDomainService, CategoryDomainService>();
        return services;
    }
}
