using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public record CategoryVariantAttributeDetailedResponse (
    Guid CategoryId,
    Guid VariantAttributeId,
    string Name,
    string Code,
    string Datatype,
    ValuesJson? AllowedValues,
    bool AffectedInventory,
    short DisplayOrder,
    bool IsRequired,
    DateTime CreatedAt)
{
    private CategoryVariantAttributeDetailedResponse() : 
        this(Guid.Empty, 
            Guid.Empty, 
            string.Empty, 
            string.Empty, 
            string.Empty, 
            null, 
            true, 
            0, 
            true, 
            DateTime.UtcNow) {}
}