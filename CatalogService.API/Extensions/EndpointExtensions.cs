using Asp.Versioning;
using Asp.Versioning.Builder;
using CatalogService.API.Endpoints;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace CatalogService.API.Extensions;
public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpoints = [.. assembly
            .DefinedTypes
            .Where(type => type.IsClass && !type.IsInterface && !type.IsAbstract && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))];


        services.TryAddEnumerable(endpoints);

        return services;
    }


    public static IApplicationBuilder MapEndpoints(
        this WebApplication app,
        RouteGroupBuilder? routeGroupBuilder = null
        )
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .HasApiVersion(new ApiVersion(2, 0))
            .ReportApiVersions()
            .Build();

        var versionedGroup = app.MapGroup("api/v{apiVersion:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        IEndpointRouteBuilder builder = routeGroupBuilder is null 
            ? versionedGroup : routeGroupBuilder;

        
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }

    

}
