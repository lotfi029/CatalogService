using API;
using Scalar.AspNetCore;
using Carter;
using System.Dynamic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddAPIServices();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

//app.UseSerilogRequestLogging();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.Run();
