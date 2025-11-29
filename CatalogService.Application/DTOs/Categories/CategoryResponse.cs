namespace CatalogService.Application.DTOs.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    string Slug,
    Guid ParentId,
    short Level,
    string? Path);