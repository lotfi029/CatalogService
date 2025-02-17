using API.Infrastructure;
using API.OpenApi;
using Application;
using Infrastructure;

namespace API;
public static class DependencyInjection
{
    public static void AddAPIServices(this IHostApplicationBuilder builder)
    {
        builder.AddInfrastructureServices();
        builder.AddApplicationServices();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSignalR();

        builder.Services.AddCorsPolicy();
        
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        builder.Services.AddCarter();

    }
    private static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AngularClient",
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:4200")
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowCredentials();
                        });
        });
    }
}
