namespace CatalogService.Core.ValueObjects;

public record Price 
{
    public decimal Amount { get; set; } = 0.0m;
    public string CurrencyType { get; set; } = string.Empty;

    public Price()
    {
        Amount = 0.0m;
        CurrencyType = string.Empty;
    }
    public Price(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        CurrencyType = "USD";
    }
    public Price(decimal amount, string currencyType)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currencyType))
            throw new ArgumentException("Currency type is required", nameof(currencyType));

        Amount = amount;
        CurrencyType = currencyType.ToUpperInvariant();
    }
}