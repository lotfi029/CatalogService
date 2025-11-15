namespace CatalogService.Domain.Entities;

public class ProductAttributes
{
    public Guid ProductId { get; set; }
    public Guid AttributeId {  get; set; }
    public string Value { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    public Product Product { get; set; } = default!;
    public Attribute Attribute { get; set; } = default!;
}