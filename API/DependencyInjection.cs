using API.Infrastructure;
using API.OpenApi;
using Application;
using Infrastructure;

namespace API;
public static class DependencyInjection
{
    public static void AddAPIServices(this IHostApplicationBuilder builder)
    {
        builder.AddApplicationServices();
        builder.AddInfrastructureServices();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        builder.Services.AddCarter();
    }
}
