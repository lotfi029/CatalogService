namespace CatalogService.Core.ValueObjects;

public class Price
{
    public decimal Amount { get; set; } = 0.0m;
    public string Currency { get; set; } = string.Empty;
}
