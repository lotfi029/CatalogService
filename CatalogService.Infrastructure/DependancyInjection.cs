using CatalogService.Application.Features.Categories.Queries;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IVariantAttributeRepository, VariantAttributeRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICategoryVariantAttributeRepository, CategoryVariantAttributeRepository>();
        
        services.AddHealthChecks()
            .AddNpgSql(name: "ApplicationDb", connectionString: connectionString!);


        services.Configure<DapperOptions>(opt =>
        {
            opt.ConnectionString = connectionString!;
        });
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IVariantAttributeQueries, VariantAttributeQueries>();

        DapperConfiguration.Configure();

        return services;
    }
}
