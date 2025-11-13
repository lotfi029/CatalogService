namespace CatalogService.Core.Entities;

public class CategoryVariantAttribute
{
    public Guid CategoryId { get; set; }
    public Guid VariantAttributeId { get; set; }

    public bool IsRequired { get; set; } = false;
    public short DisplayOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    public Category Category { get; set; } = default!;
    public VariantAttributeDefinition VariantAttribute { get; set; } = default!;
}