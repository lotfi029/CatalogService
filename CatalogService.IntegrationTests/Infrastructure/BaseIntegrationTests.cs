using CatalogService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
{
    protected IServiceScope Scope { get; private init; } = null!;
    protected ApplicationDbContext AppDbContext { get; private init; } = null!;
    
    public BaseIntegrationTests(IntegrationTestWebAppFactory factory)
    {
        Scope = factory.Services.CreateScope();
        AppDbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (AppDbContext.Database.GetPendingMigrations().Any())
        {
            AppDbContext.Database.Migrate();
        }
    }
}
