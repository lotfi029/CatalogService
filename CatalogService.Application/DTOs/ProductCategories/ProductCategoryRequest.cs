using CatalogService.Application.DTOs.ProductVariants;

namespace CatalogService.Application.DTOs.ProductCategories;

public sealed record ProductCategoryRequest(
    bool? IsPrimary,
    List<ProductVariantRequest> CategoryVariants);
