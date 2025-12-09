using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.DTOs.ProductVariants;

public sealed record UpdateProductVariantCustomOptionsRequest(ProductVariantsOption CustomVariant);
