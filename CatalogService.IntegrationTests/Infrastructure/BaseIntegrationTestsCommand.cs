using CatalogService.Application.Abstractions.Messaging;

namespace CatalogService.IntegrationTests.Infrastructure;

public abstract class BaseIntegrationTestsCommand<TCommand>(IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
    where TCommand : ICommand
{
    protected ICommandHandler<TCommand> CommandHandler { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        CommandHandler = GetService<ICommandHandler<TCommand>>();
    }
}
public abstract class BaseIntegrationTestsCommand<TCommand, TResponse>(
    IntegrationTestWebAppFactory factory) : BaseIntegrationTests(factory)
    where TCommand : ICommand<TResponse>
{
    protected ICommandHandler<TCommand, TResponse> CommandHandler { get; private set; } = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        CommandHandler = GetService<ICommandHandler<TCommand, TResponse>>();
    }
}
