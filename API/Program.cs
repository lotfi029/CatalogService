using API;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddAPIServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
