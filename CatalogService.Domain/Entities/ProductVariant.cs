using CatalogService.Domain.JsonProperties;
using System.Security.Cryptography;

namespace CatalogService.Domain.Entities;

public sealed class ProductVariant
{
    public Guid Id { get; }
    public Guid ProductId { get; private set; }
    public Sku SKU { get; } = default!;
    public Money Price { get; private set; } = new();
    public Money? CompareAtPrice { get; private set; } = new();

    public Product Product { get; private set; } = default!;
    public bool IsDeleted { get; private set; }

    public ICollection<ProductVariantValue> Values { get; init; } = [];
    private ProductVariant() { }
    private ProductVariant(
        Guid productId,
        Sku sku,
        Money price,
        Money compareAtPrice
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
        Dictionary<string, string> variantAttributes,
        Money price,
        Money compareAtPrice
        )
    {

        return new ProductVariant(
            productId,
            GenerateSku(productId, variantAttributes),
            price,
            compareAtPrice
            );
    }
    private static Sku GenerateSku(Guid productId, Dictionary<string, string> variantAttributes)
    {
        var prefix = productId.ToString("N")[..8];

        prefix = prefix.ToLower();

        if (variantAttributes.Count == 0)
            return new Sku($"{prefix}");

        var variantParts = variantAttributes
        .Select(o =>
        {
            var keyPart = new string([.. o.Key
                .Where(char.IsLetterOrDigit)
                .Take(3)])
                .ToUpper();

            var valuePart = new string([.. o.Value.Where(char.IsLetterOrDigit)])
                .ToUpper();

            return $"{keyPart}-{valuePart}";
        });

        var variantString = string.Join("_", variantParts);
        return new($"{prefix}_{variantString}");
    }
}
