namespace CatalogService.Core.Entities;

public class ProductCategories
{
    public Guid ProductId { get; set; }
    public Guid CategoryId  { get; set; }
    public bool IsPrimary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public Product Product { get; set; } = default!;
    public Category Category { get; set; } = default!;
}