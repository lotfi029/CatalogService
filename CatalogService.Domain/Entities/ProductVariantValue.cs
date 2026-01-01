namespace CatalogService.Domain.Entities;

public sealed class ProductVariantValue
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public Guid ProductVariantId { get; }
    public Guid VariantAttributeId { get; }
    public string Value { get; } = string.Empty;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public bool IsDeleted { get; private set; } = false;
    public ProductVariant ProductVariant { get; init; } = default!;
    public VariantAttributeDefinition VariantAttributeDefinition { get; init; } = default!;
    private ProductVariantValue() { }
    private ProductVariantValue(
        Guid productVariantId,
        Guid variantAttributeId,
        string value)
    {
        ProductVariantId = productVariantId;
        VariantAttributeId = variantAttributeId;
        Value = value;
    }

    public static ProductVariantValue Create(
        Guid productVariantId,
        Guid variantAttributeId,
        string value)
    {
        return new(
            productVariantId: productVariantId,
            variantAttributeId: variantAttributeId,
            value: value);
    }

    public void Delete()
        => IsDeleted = true;
}