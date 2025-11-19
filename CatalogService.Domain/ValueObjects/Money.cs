namespace CatalogService.Domain.ValueObjects;

public record Money 
{
    public decimal Amount { get; set; } = 0.0m;
    public string CurrencyType { get; set; } = string.Empty;

    public Money()
    {
        Amount = 0.0m;
        CurrencyType = "USD";
    }
    public Money(decimal? amount)
        : this()
    {
        if (amount is null || amount < 0)
            throw new ArgumentException("Amount can't be null or negative value", nameof(amount));

        Amount = amount.Value;
        CurrencyType = "USD";
    }
    public Money(decimal? amount, string? currencyType)
        : this(amount)
    {
        if (string.IsNullOrWhiteSpace(currencyType))
            throw new ArgumentException("Currency type is required", nameof(currencyType));

        CurrencyType = currencyType.ToUpperInvariant();
    }
}