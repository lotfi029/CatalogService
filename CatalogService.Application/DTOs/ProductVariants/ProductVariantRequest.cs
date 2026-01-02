namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record ProductVariantRequest(
    Guid ProductId,
    decimal Price,
    decimal? CompareAtPrice,
    IEnumerable<VariantValueRequest> Variants
    );
