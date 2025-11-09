namespace CatalogService.Core.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string VendorId { get; set; } = string.Empty;
    public string BaseSKU { get; set; } = string.Empty;
    public Price BasePrice { get; set; } = default!;
    public ProductStatus Status { get; set; } = ProductStatus.draft;
}
