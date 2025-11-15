namespace CatalogService.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public string? SKU { get; set; } = string.Empty;
    public Enums.ProductStatus Status { get; set; } = Enums.ProductStatus.Unspecified;
    public Dictionary<string, object>? Metadata {  get; set; }

    public ICollection<ProductAttributes> ProductAttributes { get; set; } = [];
    public ICollection<ProductVariant> ProductVariants { get; set; } = [];
    public ICollection<ProductCategories> ProductCategories { get; set; } = [];
}
