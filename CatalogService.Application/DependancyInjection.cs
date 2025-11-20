using CatalogService.Application.DTOs.Categories;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddServices();
        services.AddCQRS();
        return services;
    }
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependancyInjection).Assembly);
        services.AddSingleton<IMapper>(new Mapper(config));

        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequest>(includeInternalTypes: true);

        return services;
    }
    private static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependancyInjection))
            //.AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            //.AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            //    .AsImplementedInterfaces()
            //    .WithScopedLifetime()
        );

        

        return services;
    }
}
