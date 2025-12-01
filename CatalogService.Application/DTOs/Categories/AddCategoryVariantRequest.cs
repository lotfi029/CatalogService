namespace CatalogService.Application.DTOs.Categories;

public sealed record AddCategoryVariantRequest(
    Guid VariantId, 
    short DisplayOrder,
    bool IsRequired);
