using CatalogService.Domain.ValueObjects;
using FluentAssertions;

namespace Domain.UnitTests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void DefaultConstructor_Should_SetDefaultValues()
    {
        var money = new Money();

        money.Amount.Should().Be(0.0m);
        money.CurrencyType.Should().Be("USD");
    }
    [Fact]
    public void Constructor_Should_CreateMoney_WhenValidAmountAndCurrency()
    {
        decimal amount = 20m;
        string currency = "eur";

        var money = new Money(amount, currency);

        money.Amount.Should().Be(amount);
        money.CurrencyType.Should().Be("EUR");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(-1d)]
    public void Constructor_Should_ThrowArgumentException_WhenInvalidAmountValue(double? amountValue)
    {
        decimal? amount = amountValue is null ? null : (decimal?)amountValue;

        Money Action() => new(amount);

        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should().Be("amount");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10.5)]
    public void Constructor_Should_CreateMoney_WhenValidAmount(double amountValue)
    {
        decimal amount = (decimal)amountValue;

        var money = new Money(amount);

        money.Amount.Should().Be(amount);
        money.CurrencyType.Should().Be("USD");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void Constructor_Should_ThrowArgumentException_WhenInvalidCurrencyType(string? currency)
    {
        decimal? amount = 10m;

        Money Action() => new(amount, currency);

        FluentActions.Invoking(Action)
            .Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should().Be("currencyType");
    }
}
