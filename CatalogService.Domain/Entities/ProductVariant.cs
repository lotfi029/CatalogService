namespace CatalogService.Domain.Entities;

public class ProductVariant : AuditableEntity
{
    public Guid ProductId { get; private set; }
    public Sku SKU { get; private set; } = default!;
    public string VariantAttributes {  get; private set; } = string.Empty; // json
    public string CustomizationOptions {  get; private set; } = string.Empty; // json


    public Money Price { get; private set; } = new();
    public Money CompareAtPrice { get; private set; } = new();

    public Product Product { get; private set; } = default!;

    private ProductVariant() { }
    private ProductVariant(
        Guid productId,
        Sku sku,
        string variantAttributes,
        string customizationOptions,
        Money price,
        Money compareAtPrice 
        ) : base()
    {
        ProductId = productId;
        SKU = sku;
        VariantAttributes = variantAttributes;
        CustomizationOptions = customizationOptions;
        Price = price;
        CompareAtPrice = compareAtPrice;
    }

    public static ProductVariant Create(
        Guid productId,
        Sku sku,
        string variantAttributes,
        string customizationOptions,
        Money price,
        Money compareAtPrice 
        )
    {
        return new ProductVariant(
            productId,
            sku,
            variantAttributes,
            customizationOptions,
            price,
            compareAtPrice
            );
    }

    public void ChangePrice(Money newPrice)
    {
        Price = newPrice;
    }
    public void ChangeCompareAtPrice(Money newCompareAtPrice)
    {
        CompareAtPrice = newCompareAtPrice;
    }
}
