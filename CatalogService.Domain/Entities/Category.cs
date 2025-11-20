using CatalogService.Domain.DomainEvents.Categories;

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


    private readonly List<CategoryVariantAttribute> _variantAttributes = [];
    public IReadOnlyCollection<CategoryVariantAttribute> CategoryVariantAttributes => _variantAttributes.AsReadOnly();



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

        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(slug));

        if (level < 0)
            throw new ArgumentException("level can't be negative value", nameof(level));

        var category = new Category(
            name,
            slug,
            level,
            parentId,
            description,
            path,
            metadata);

        category.AddDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return category;
    }

    public void UpdateMetadata(Dictionary<string, object> metadata)
    {
        Metadata = metadata;
    }

    public void UpdateDetails(string name, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
        if (description is not null)
            Description = description;
    }

    public void AddVariantAttribute(CategoryVariantAttribute categoryVariantAttribute)
    {
        ArgumentNullException.ThrowIfNull(categoryVariantAttribute);

        _variantAttributes.Add(categoryVariantAttribute);
    }
}

