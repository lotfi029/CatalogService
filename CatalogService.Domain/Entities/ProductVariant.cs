namespace CatalogService.Domain.Entities;

public sealed class ProductVariant
{
    public Guid Id { get; }
    public Guid ProductId { get; private set; }
    public string SKU { get; } = string.Empty;
    public Money Price { get; private set; } = new();
    public Money? CompareAtPrice { get; private set; } = new();

    public Product Product { get; private set; } = default!;
    public bool IsDeleted { get; private set; }

    public ICollection<ProductVariantValue> Values { get; init; } = [];
    private ProductVariant() { }
    private ProductVariant(
        Guid productId,
        string sku,
        Money price,
        Money? compareAtPrice
        ) : base()
    {
        Id = Guid.CreateVersion7();
        ProductId = productId;
        SKU = sku;
        Price = price;
        CompareAtPrice = compareAtPrice;
        IsDeleted = false;
    }

    public static ProductVariant Create(
        Guid productId,
        List<string> variants,
        Money price,
        Money? compareAtPrice
        )
    {

        return new ProductVariant(
            productId,
            GenerateSku(productId, variants),
            price,
            compareAtPrice
            );
    }
    public Result UpdatePrice(decimal price, decimal? compareAtPrice, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            return DomainErrors.Null("Currency");

        if (price < 0)
        {
            return DomainErrors.Null("Price");
        }
        if (compareAtPrice is not null)
        {
            if (compareAtPrice.Value < 0)
                return DomainErrors.Null("CompareAtPrice");

            CompareAtPrice = new(compareAtPrice, currency);
        }
        Price = new(price, currency);
        return Result.Success();
    }
    private static string GenerateSku(Guid productId, List<string> variantAttributes)
    {
        var productPrefix = productId.ToString("N")[..6].ToUpperInvariant();

        if (variantAttributes == null || variantAttributes.Count == 0)
            return productPrefix;
        variantAttributes = variantAttributes.Count > 3 ? [.. variantAttributes.Take(3)] : variantAttributes;
        var variantCode = string.Join("-", variantAttributes
            .Select(a => a.Length > 2 ? a[..2].ToUpperInvariant() : a.ToUpperInvariant()));

        return $"{productPrefix}-{variantCode}";
    }
}
