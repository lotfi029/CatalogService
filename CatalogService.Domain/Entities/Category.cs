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
    public Category? Parent { get; private set; }

    private readonly List<Category> _children = [];
    private readonly List<CategoryVariantAttribute> _variantAttributes = [];
    public IReadOnlyCollection<CategoryVariantAttribute> CategoryVariantAttributes => _variantAttributes.AsReadOnly();
    public IReadOnlyCollection<Category> Children => _children.AsReadOnly(); 
    private Category() { }
    private Category(
        string name,
        string slug,
        short level,
        bool isActive,
        Guid? parentId = null,
        string? description = null,
        string? path = null)
        : base()
    {
        Name = name;
        Slug = slug;
        Level = level;
        ParentId = parentId;
        Description = description;
        Path = path;

        if (isActive)
            Active();
        else
            Deactive();
    }

    public static Category Create(
        string name,
        string slug,
        short level,
        bool isActive,
        Guid? parentId = null,
        string? description = null,
        string? path = null)
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(slug)); // make it domain exception

        if (level < 0)
            throw new ArgumentException("level can't be negative value", nameof(level));

        var category = new Category(
            name,
            slug,
            level,
            isActive,
            parentId,
            description,
            CreatePath(path, slug));

        category.AddDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return category;
    }
    public static string CreatePath(string? parentPath, string slug)
    {
        if (!string.IsNullOrWhiteSpace(parentPath))
            return $"{parentPath}/{slug}";

        return slug;
    }
    public void MoveCategory(Guid? parentId, string? parentPath, short parentLevel)
    {
        if (parentLevel < 0)
            throw new ArgumentException("Category.MoveCategory the prevLevel can't be negative", nameof(parentLevel));

        ParentId = parentId.HasValue ? parentId : ParentId;
        Path = CreatePath(parentPath, Slug);
        Level = (short)(parentLevel + 1);

        AddDomainEvent(new CategoryMovedDomainEvent(Id));
    }
    public void UpdateDetails(string name, string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
        if (description is not null)
            Description = description;

        AddDomainEvent(new CategoryDetailsUpdatedDomainEvent(Id));
    }
    public void AddChild(Category child)
        => _children.Add(child);
    public void AddVariantAttribute(CategoryVariantAttribute categoryVariantAttribute)
    {
        ArgumentNullException.ThrowIfNull(categoryVariantAttribute);

        _variantAttributes.Add(categoryVariantAttribute);
    }
}

