namespace CatalogService.Application.DTOs.Categories;

public record CategoryDetailedResponse(
    Guid Id,
    string Name,
    string Slug,
    short Level,
    string? Description,
    string? Path
    );
