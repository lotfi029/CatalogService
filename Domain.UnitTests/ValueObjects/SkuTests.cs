using CatalogService.Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class SkuTests
{
    [Theory]
    [InlineData("ABCDEFGH")]
    [InlineData("12345678")]
    [InlineData("A1B2C3D4")]
    public void Create_WithValidValue_Should_ReturnSku(string value)
    {
        Sku Action() => Sku.Create(value);
        
        FluentActions.Invoking(Action)
            .Should()
            .NotBeNull();

        Action().Value.Should().Be(value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Created_NullOrWhiteSpace_Should_ThrowArgumentException(string? value)
    {
        Sku Action() => Sku.Create(value);

        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should().Be(nameof(value));
    }
    
    [Theory]
    [InlineData("AAD")]
    [InlineData("ADAFADFADAF")]
    public void Created_InvalidLenght_Should_ThrowInvalidOperationException(string? value)
    {
        Sku Action() => Sku.Create(value);

        FluentActions.Invoking(Action)
            .Should()
            .Throw<InvalidOperationException>();
    }


}
