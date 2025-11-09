namespace CatalogService.Core.Entities;

public class ProductVariant : BaseEntity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string VariantAttributes {  get; set; } = string.Empty;
    public Price Price { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
