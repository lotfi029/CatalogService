using CatalogService.Application;
using CatalogService.Application.Abstractions.Messaging;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureTests.Application.Features;

public class Commands
{
    private static Assembly applicationAssembly => typeof(ApplicationAssemblyReference).Assembly;

    [Fact]
    public void Commands_Should_BeSealed_And_NameEndWithCommand()
    {
        var commandResult = Types.InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Or()
            .ImplementInterface(typeof(ICommand<>))
            .GetTypes();

        var failedTypes = new List<Type>();

        foreach (var command in commandResult)
        {
            var validCommand = command.IsSealed && command.Name.EndsWith("Command");
            if (!validCommand)
                failedTypes.Add(command);
        }

        failedTypes.Should().BeEmpty();
    }
    [Fact]
    public void CommandHandler_Should_BeSealed_And_NotPublic_NameEndWithCommandHandler()
    {
        var commandHandlers = Types.InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .GetTypes();

        var failedTypes = new List<Type>();

        foreach (var handler in commandHandlers)
        {
            var validHandler = handler.IsSealed &&
                handler.IsNotPublic &&
                handler.Name.EndsWith("CommandHandler");

            if (!validHandler) failedTypes.Add(handler);
        }

        failedTypes.Should().BeEmpty();
    }
}
