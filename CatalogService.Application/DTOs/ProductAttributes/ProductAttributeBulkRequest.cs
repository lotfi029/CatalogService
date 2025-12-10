namespace CatalogService.Application.DTOs.ProductAttributes;

public sealed record ProductAttributeBulkRequest(IEnumerable<ProductAttributeBulk> Attributes);

