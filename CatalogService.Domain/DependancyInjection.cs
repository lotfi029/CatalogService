using CatalogService.Domain.DomainService.Attributes;
using CatalogService.Domain.DomainService.Categories;
using CatalogService.Domain.DomainService.Products;
using CatalogService.Domain.DomainService.VariantAttributes;
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
        services.AddScoped<IProductDomainService, ProductDomainService>();
        services.AddScoped<IAttributeDomainService, AttributeDomainService>();
        services.AddScoped<IVariantAttributeDomainService, VariantAttributeDomainService>();
        return services;
    }
}
