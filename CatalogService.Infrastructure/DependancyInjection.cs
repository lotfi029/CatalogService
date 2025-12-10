using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.CategoryVariants.Queries;
using CatalogService.Application.Features.ProductAttributes.Queries;
using CatalogService.Application.Features.ProductCategories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Features.ProductVariants.Queries;
using CatalogService.Application.Features.VariantAttributes.Queries;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.Dapper;
using CatalogService.Infrastructure.Persistence.Dapper.Queries;
using CatalogService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CatalogService.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionStringOrThrow();
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryVariantAttributeRepository, CategoryVariantAttributeRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
        
        services.AddScoped<IAttributeRepository, AttributeRepository>();
        services.AddScoped<IVariantAttributeRepository, VariantAttributeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        
        services.AddHealthChecks()
            .AddNpgSql(name: "ApplicationDb", connectionString: connectionString!);


        services.Configure<DapperOptions>(opt =>
        {
            opt.ConnectionString = connectionString!;
        });
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<IProductCategoryQueries, ProductCategoryQueries>();
        services.AddScoped<IProductAttributeQueries, ProductAttributeQueries>();
        services.AddScoped<IProductVariantQueries, ProductVariantQueries>();
        services.AddScoped<IAttributeQueries, AttributeQueries>();
        services.AddScoped<ICategoryVariantAttributeQueries, CategoryVariantAttributeQueries>();
        services.AddScoped<IVariantAttributeQueries, VariantAttributeQueries>();

        DapperConfiguration.Configure();

        return services;
    }
}
