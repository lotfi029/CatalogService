namespace CatalogService.Domain.Entities;

public class ProductVariant : AuditableEntity
{
    public Guid ProductId { get; set; }
    public required Sku SKU { get; set; }
    public string VariantAttributes {  get; set; } = string.Empty; // json
    public string CustomizationOptions {  get; set; } = string.Empty; // json


    public Price Price { get; set; } = new();
    public Price CompareAtPrice { get; set; } = new();

    public Product Product { get; set; } = default!;
}
