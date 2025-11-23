using CatalogService.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.IntegrationTests.Infrastructure;

public class BaseIntegrationTestsQuery<TQuery, TResponse> : BaseIntegrationTests
    where TQuery : IQuery<TResponse>
{
    private readonly IQueryHandler<TQuery, TResponse> queryHandler;

    public BaseIntegrationTestsQuery(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        queryHandler = _scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
    }
}