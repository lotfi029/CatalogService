namespace CatalogService.Application.DTOs.Categories;

public record CategoryDetailedResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid? ParentId,
    short Level,
    string? Description,
    string? Path
    );
