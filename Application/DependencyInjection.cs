using Application.Features.Auth.Handlers;
using Application.Features.Auth.Validators;
using Application.Mapping;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application;
public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationConfig();
        builder.Services.AddMapsterConfig();
        builder.Services.RegisterMediatR();
    }
    private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(MappingConfiguration).Assembly);

        services.AddSingleton<IMapper>(new Mapper(config));

        return services;
    }
    private static IServiceCollection RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());

        return services;
    }
    
}
