namespace CatalogService.Domain.ValueObjects;

public record Sku
{
    private const int DefaultLength = 8;
    private Sku(string vlaue)
        => Value = vlaue;
    public string Value { get; private init; }
    
    public static Sku? Create(string value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        if (value.Length != DefaultLength)
            return null;

        return new Sku(value);
    }
}




// outbox pattern