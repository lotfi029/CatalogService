using API;
using Scalar.AspNetCore;
using API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddAPIServices();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoint();


app.Run();
