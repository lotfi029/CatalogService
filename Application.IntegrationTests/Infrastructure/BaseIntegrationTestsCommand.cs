using CatalogService.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTestsCommand<TCommand> : BaseIntegrationTests
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> commandHandler;

    public BaseIntegrationTestsCommand(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        commandHandler = _scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
    }
}
public abstract class BaseIntegrationTestsCommand<TCommand, TResponse> : BaseIntegrationTests
    where TCommand : ICommand<TResponse>
{
    protected readonly ICommandHandler<TCommand, TResponse> commandHandler;

    public BaseIntegrationTestsCommand(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        commandHandler = _scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();   
    }
}
