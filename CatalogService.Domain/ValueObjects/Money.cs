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
    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        CurrencyType = "USD";
    }
    public Money(decimal amount, string currencyType)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currencyType))
            throw new ArgumentException("Currency type is required", nameof(currencyType));

        Amount = amount;
        CurrencyType = currencyType.ToUpperInvariant();
    }


}