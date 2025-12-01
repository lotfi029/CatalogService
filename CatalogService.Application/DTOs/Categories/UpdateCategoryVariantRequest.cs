namespace CatalogService.Application.DTOs.Categories;

public sealed record UpdateCategoryVariantRequest(
    short DisplayOrder,
    bool IsRequired);
