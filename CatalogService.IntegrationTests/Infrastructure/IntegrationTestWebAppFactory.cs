using CatalogService.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CatalogService.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("catalog")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();
    private readonly SemaphoreSlim _resetLock = new(1, 1);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descripter = services
                .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descripter is not null)
            {
                services.Remove(descripter);
            }

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbContainer.GetConnectionString());
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(dataSource);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
    public async Task ResetDatabaseAsync()
    {
        await _resetLock.WaitAsync();
        try
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tableNames = context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Where(t => t != null && t != "__EFMigrationsHistory")
                .Distinct()
                .ToList();

            if (tableNames.Count == 0)
                return;

            
            await context.Database.ExecuteSqlAsync(
                $"SET session_replication_role = 'replica';");

            foreach (var tableName in tableNames)
            {
                await context.Database.ExecuteSqlRawAsync(
                    $"TRUNCATE TABLE \"{tableName}\" RESTART IDENTITY CASCADE;");
            }

            await context.Database.ExecuteSqlRawAsync(
                "SET session_replication_role = 'origin';");
        }
        finally
        {
            _resetLock.Release();
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        _resetLock.Dispose();
        await _dbContainer.StartAsync();
    }
}