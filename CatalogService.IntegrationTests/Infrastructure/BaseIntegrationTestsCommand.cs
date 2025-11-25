using CatalogService.Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTestsCommand<TCommand> : BaseIntegrationTests
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> commandHandler;

    public BaseIntegrationTestsCommand(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        commandHandler = Scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();
    }
}
public abstract class BaseIntegrationTestsCommand<TCommand, TResponse> : BaseIntegrationTests
    where TCommand : ICommand<TResponse>
{
    protected readonly ICommandHandler<TCommand, TResponse> CommandHandler;

    public BaseIntegrationTestsCommand(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        CommandHandler = Scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();   
    }
}
