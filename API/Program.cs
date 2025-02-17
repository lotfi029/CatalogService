using API;
using Scalar.AspNetCore;
using Serilog;
using API.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

builder.AddAPIServices();



builder.Services.AddHostedService<ServerTimeNotifyer>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseRouting();

app.UseCors("AngularClient");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapCarter();

app.Run();
