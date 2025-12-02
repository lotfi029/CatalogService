namespace CatalogService.Application.DTOs.CategoryVariantAttributes;

public sealed record AddCategoryVariantBulkRequest(
    ICollection<AddCategoryVariantRequest> Variants);