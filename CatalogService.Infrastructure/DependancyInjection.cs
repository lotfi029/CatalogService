using CatalogService.Infrastructure.Persistence.Contexts;
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
        dataSourceBuilder.EnableDynamicJson(); // Enable dynamic JSON for Dictionary<string, object>
        var dataSource = dataSourceBuilder.Build();

        // Use the configured data source
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(dataSource));
        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseNpgsql(connectionString);
        //});

        return services;
    }
}
