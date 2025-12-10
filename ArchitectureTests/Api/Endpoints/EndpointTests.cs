using CatalogService.API.Endpoints;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace ArchitectureTests.Api.Endpoints;

public class EndpointTests
{
    private static Assembly apiAssembly => typeof(Program).Assembly;

    [Fact]
    public void Endpoint_Should_BeSealed()
    {
        var result = Types.InAssembly(apiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Endpoint_Should_BeNotPublic()
    {
        var result = Types.InAssembly(apiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .NotBePublic()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Endpoint_Should_HaveEndpointPostFix()
    {
        var result = Types.InAssembly(apiAssembly)
            .That()
            .ImplementInterface(typeof(IEndpoint))
            .Should()
            .HaveNameEndingWith("Endpoints")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}