using CatalogService.API;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAPI(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("api/products", async (ApplicationDbContext context, CancellationToken ct) =>
{
   var products = await context.Products.ToListAsync(ct);
    return Results.Ok(products);
});

app.Run();