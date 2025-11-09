namespace CatalogService.Core.Entities;

public class ProductAttributes
{
    public Guid ProductId { get; set; }
    public Guid AttributeId {  get; set; }
    public string Value { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
