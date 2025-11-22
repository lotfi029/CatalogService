namespace CatalogService.Application.DTOs.Products;

public sealed record CreateProductRequest(
    string Name,
    string Status,
    string VendorId,
    string Sku,
    string? Description
    );
