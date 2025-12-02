namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed record AddCategoryVariantRequest(
    Guid VariantId, 
    short DisplayOrder,
    bool IsRequired);