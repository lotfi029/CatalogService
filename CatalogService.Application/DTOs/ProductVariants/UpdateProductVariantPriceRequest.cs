namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record UpdateProductVariantPriceRequest(
    decimal Price, 
    decimal? CompareAtPrice, 
    string Currency);
