using CatalogService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTests : IClassFixture<IntegrationTestWebAppFactory>
{
    protected IServiceScope _scope;
    private readonly ApplicationDbContext dbContext;

    public BaseIntegrationTests(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
