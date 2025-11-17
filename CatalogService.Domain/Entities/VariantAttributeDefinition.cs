namespace CatalogService.Domain.Entities;

public class VariantAttributeDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    
    public bool IsRequired { get; set; } = false;
    public bool AffectsInventory { get; set; } = false;
    public bool AffectsPricing { get; set; } = false;

    public short DisplayOrder { get; set; } = 0;

    public Dictionary<string, object>? AllowedValues { get; set; }
    public Dictionary<string, object>? ValidationRules { get; set; }

    public ICollection<CategoryVariantAttribute> CategoryVariantAttributes { get; set; } = [];

}
