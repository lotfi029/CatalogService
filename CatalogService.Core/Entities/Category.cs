namespace CatalogService.Core.Entities;

public class Category : Entity
{
    public Guid? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? Path {  get; set; } = string.Empty;
    public short Level { get; set; } = 0;
    public string? Metadata { get; set; }
    public Category? Parent { get; set; }

    public ICollection<ProductCategories> ProductCategories { get; set; } = [];
}

