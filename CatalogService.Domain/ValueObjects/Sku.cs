namespace CatalogService.Domain.ValueObjects;

public sealed record Sku
{
    public const int DefaultLength = 8;
    public Sku(string vlaue)
        => Value = vlaue;
    public string Value { get; private init; }
}