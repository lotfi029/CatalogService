namespace CatalogService.Application.DTOs.Products;

public sealed record ProductRequest(
    string Name,
    string? Description
    );