using CatalogService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTests(IntegrationTestWebAppFactory factory) : IClassFixture<IntegrationTestWebAppFactory>, IAsyncLifetime
{
    protected IServiceScope Scope { get; private set; } = null!;
    protected ApplicationDbContext AppDbContext { get; private set; } = null!;
    
    public virtual async Task InitializeAsync()
    {
        await factory.ResetDatabaseAsync();

        Scope = factory.Services.CreateScope();
        AppDbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    public Task DisposeAsync()
    {
        Scope?.Dispose();
        return Task.CompletedTask;
    }

    protected T GetService<T>() where T : notnull
    {
        return Scope.ServiceProvider.GetRequiredService<T>();
    }
}