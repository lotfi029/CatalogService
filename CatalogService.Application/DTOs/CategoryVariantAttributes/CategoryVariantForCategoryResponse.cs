namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public record CategoryVariantForCategoryResponse(
    Guid VariantAttributeId,
    string VariantAttributeName,
    string Code,
    string Datatype,
    short DisplayOrder,
    bool IsRequired
    );