using CatalogService.Application.Abstractions.Messaging;

namespace CatalogService.IntegrationTests.Infrastructure;

public class BaseIntegrationTestsQuery<TQuery, TResponse>(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
    where TQuery : IQuery<TResponse>
{
    protected IQueryHandler<TQuery, TResponse> QueryHandler { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        QueryHandler = GetService<IQueryHandler<TQuery, TResponse>>();
    }
}