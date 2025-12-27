using Asp.Versioning.ApiExplorer;
using CatalogService.API;
using CatalogService.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddAPI(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });
    await app.Services.InitialElasticsearch();
}
Console.WriteLine(app.Environment.EnvironmentName);

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();
app.MapEndpoints();
app.Run();   