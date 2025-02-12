using API;
using Scalar.AspNetCore;
using Serilog;
using API.Hubs;
using API.BackgroundServices;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

builder.AddAPIServices();

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<ServerTimeNotifyer>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<ProductHub>("/product-hub");
app.MapHub<NotificationHub>("/notification-hub");

app.MapCarter();

app.Run();
