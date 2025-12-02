namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed record UpdateCategoryVariantRequest(
    short DisplayOrder,
    bool IsRequired);
