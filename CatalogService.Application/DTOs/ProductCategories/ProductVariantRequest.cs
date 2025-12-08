using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductCategories;

public sealed record ProductVariantRequest(
    decimal Price,
    decimal? CompareAtPrice,
    ProductVariantsOption Variants
    );
