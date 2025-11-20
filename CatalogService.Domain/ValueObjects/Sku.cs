namespace CatalogService.Domain.ValueObjects;

public sealed record Sku
{
    private const int DefaultLength = 8;
    private Sku(string vlaue)
        => Value = vlaue;
    public string Value { get; private init; }
    
    public static Sku Create(string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (value.Length != DefaultLength)
            throw new InvalidOperationException($"Sku must be {DefaultLength} characters long.");

        return new Sku(value);
    }
}