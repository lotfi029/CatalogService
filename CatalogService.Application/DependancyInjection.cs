using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();
        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependancyInjection).Assembly);
        services.AddSingleton<IMapper>(new Mapper(config));

        return services;
    }
}
