using Asp.Versioning;
using CatalogService.API.Extensions;
using CatalogService.Application;
using CatalogService.Domain;
using CatalogService.Infrastructure;
using System.Timers;

namespace CatalogService.API;

public static class DependancyInjection
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerGenOptions>();

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options => 
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        }); 
        
        services.AddAntherLayers(configuration);
        services.AddEndpoints(typeof(DependancyInjection).Assembly);

        services.AddHealthChecks();

        return services;
    }
    private static IServiceCollection AddAntherLayers(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDomain()
            .AddInfrastructure(configuration)
            .AddApplication();


        return services;
    }
}
