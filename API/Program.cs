using API;
using Scalar.AspNetCore;
using Carter;
using System.Dynamic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

builder.AddAPIServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseRouting();

app.UseCors(options => { options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapCarter();

app.Run();
