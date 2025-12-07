using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductCategories;

public sealed record ProductCategoryRequest(
    bool? IsPrimary,
    List<ProductVariantsOption> CategoryVariants);