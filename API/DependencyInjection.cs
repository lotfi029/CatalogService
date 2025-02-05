using API.OpenApi;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.OpenApi;

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

        
    }
}
