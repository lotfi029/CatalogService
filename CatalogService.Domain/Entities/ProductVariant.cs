using CatalogService.Domain.JsonProperties;
namespace CatalogService.Domain.Entities;
public sealed class ProductVariant
{
    public Guid Id { get; }
    public Guid ProductId { get; private set; }
    public Sku SKU { get; } = default!;
    public ProductVariantsOption VariantAttributes { get; private set; } = default!;
    public ProductVariantsOption? CustomizationOptions { get; private set; }

    public Money Price { get; private set; } = new();
    public Money? CompareAtPrice { get; private set; } = new();

    public Product Product { get; private set; } = default!;

    private ProductVariant() { }
    private ProductVariant(
        Guid productId,
        Sku sku,
        ProductVariantsOption variantAttributes,
        ProductVariantsOption? customizationOptions,
        Money price,
        Money compareAtPrice 
        ) : base()
    {
        Id = Guid.CreateVersion7();
        ProductId = productId;
        SKU = sku;
        VariantAttributes = variantAttributes;
        CustomizationOptions = customizationOptions;
        Price = price;
        CompareAtPrice = compareAtPrice;
    }

    public static ProductVariant Create(
        Guid productId,
        ProductVariantsOption variantAttributes,
        ProductVariantsOption? customizationOptions,
        Money price,
        Money compareAtPrice 
        )
    {

        return new ProductVariant(
            productId,
            GenerateSku(productId, variantAttributes),
            variantAttributes,
            customizationOptions,
            price,
            compareAtPrice
            );
    }
    private static Sku GenerateSku(Guid productId, ProductVariantsOption variantAttributes)
    {
        var prefix = productId.ToString("N")[..8];
        
        prefix = prefix.ToLower();

        if (variantAttributes?.Variants is null || variantAttributes.Variants.Count == 0)
            return new Sku($"{prefix}");

        var variantParts = variantAttributes.Variants
        .OrderBy(o => o.Key)
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
