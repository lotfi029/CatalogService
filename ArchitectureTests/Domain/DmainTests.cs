using CatalogService.Domain;
using CatalogService.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using SharedKernel;
using System.Reflection;

namespace ArchitectureTests.Domain;

public class DmainTests
{
    private static readonly Assembly domainAssembly = typeof(DomainAssemblyReference).Assembly;

    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostfix()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_Should_HavePrivateParameterlessConstructor()
    {
        var entityTypes = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach(var entity in entityTypes)
        {
            if (entity.Name == nameof(AuditableEntity))
                continue;

            var constructors = entity.GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                failingTypes.Add(entity);
        }

        failingTypes.Should().BeEmpty();
    }

    [Fact]
    public void Entities_Should_NotHavePublicConstructor()
    {
        var entityTypes = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach (var entity in entityTypes)
        {
            var constructors = entity.GetConstructors(
                BindingFlags.Public | BindingFlags.Instance);

            if (constructors.Any(c => c.IsPublic))
                failingTypes.Add(entity);
        }

        failingTypes.Should().BeEmpty();
    }
    [Fact]
    public void EntityProperties_Should_HavePrivateSet()
    {
        var entityTypes = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes();

        var failingTypes = new List<Type>();

        foreach(var entity in entityTypes)
        {
            var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (properties.Any(a => a.SetMethod is not null && a.SetMethod.IsPublic))
                failingTypes.Add(entity);
        }

        failingTypes.Should().BeEmpty();
    }


}
