using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Entities;

public class Category : AuditableEntity
{
    public Guid? ParentId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string? Path {  get; private set; } = string.Empty;
    public short Level { get; private set; } = 0;
    public Dictionary<string, object>? Metadata { get; private set; }
    public Category? Parent { get; private set; }

    public ICollection<Category>? Childerns { get; set; } = [];
    public ICollection<ProductCategories> ProductCategories { get; set; } = [];
    public ICollection<CategoryVariantAttribute> CategoryVariantAttributes { get; set; } = [];



    private Category() { }
    private Category(
        string name,
        string slug,
        short level,
        Guid? parentId = null,
        string? description = null,
        string? path = null,
        Dictionary<string, object>? metadata = null)
        : base()
    {
        Name = name;
        Slug = slug;
        Level = level;
        ParentId = parentId;
        Description = description;
        Path = path;
        Metadata = metadata;
    }

    public static Category Create(
        string name,
        string slug,
        short level,
        Guid? parentId = null,
        string? description = null,
        string? path = null,
        Dictionary<string, object>? metadata = null)
    {
        return new Category(
            name,
            slug,
            level,
            parentId,
            description,
            path,
            metadata);
    }


}

