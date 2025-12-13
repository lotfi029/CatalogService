using CatalogService.Domain.Errors.EntitiesErrors;

namespace CatalogService.Domain.Entities;

public sealed class ProductAttributes
{
    public Guid ProductId { get; set; }
    public Guid AttributeId {  get; set; }
    public string Value { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // delete
    public bool IsDeleted { get; set; } = false;
    public Product Product { get; set; } = default!;
    public Attribute Attribute { get; set; } = default!;
    private ProductAttributes() { }
    private ProductAttributes(
        Guid productId,
        Guid attributeId,
        string value
        )
    {
        ProductId = productId;
        AttributeId = attributeId;
        Value = value;
    }
    public static ProductAttributes Create(
        Guid productId,
        Guid attributeId,
        string value
        )
    {
        return new ProductAttributes(
            productId,
            attributeId,
            value
            );
    }
    public Result UpdateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.Null("Value");

        Value = value;
        return Result.Success();
    }
    public Result Delete()
    {
        IsDeleted = true;
        return Result.Success();
    }
    public Result Restore()
    {
        IsDeleted = false;
        return Result.Success();
    }
}