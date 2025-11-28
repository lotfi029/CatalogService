using CatalogService.Application;
using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence;
using CatalogService.Infrastructure.Persistence.Dapper;
using CatalogService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CatalogService.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPresistence(configuration);
        return services;
    }

    private static IServiceCollection AddPresistence(this IServiceCollection services, IConfiguration configuration)
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddHealthChecks()
            .AddNpgSql(name: "ApplicationDb", connectionString: connectionString!);


        services.Configure<DapperOptions>(opt =>
        {
            opt.ConnectionString = connectionString!;
        });
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        return services;
    }
}
