namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record VariantValueRequest(
    Guid VariantId,
    string Value);
