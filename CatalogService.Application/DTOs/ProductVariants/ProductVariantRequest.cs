using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record ProductVariantRequest(
    decimal Price,
    decimal? CompareAtPrice,
    ProductVariantsOption Variants
    );
