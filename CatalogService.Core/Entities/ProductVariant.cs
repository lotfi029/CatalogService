namespace CatalogService.Core.Entities;

public class ProductVariant : Entity
{
    public Guid ProductId { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string VariantAttributes {  get; set; } = string.Empty; // json
    public string CustomizationOptions {  get; set; } = string.Empty; // json


    public Price Price { get; set; } = new();
    public Price CompareAtPrice { get; set; } = new();
    public Price CostPrice { get; set; } = new();

    public Product Product { get; set; } = default!;
}
