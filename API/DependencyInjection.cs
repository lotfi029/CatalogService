using API.OpenApi;
using Application;
using Carter;
using Infrastructure;

namespace API;
public static class DependencyInjection
{
    public static void AddAPIServices(this IHostApplicationBuilder builder)
    {
        builder.AddApplicationServices();
        builder.AddInfrastructureServices();


        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        builder.Services.AddCarter();
    }
}
