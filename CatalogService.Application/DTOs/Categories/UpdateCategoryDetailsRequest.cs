namespace CatalogService.Application.DTOs.Categories;

public sealed record UpdateCategoryDetailsRequest(
    string Name,
    string? Description
    );
