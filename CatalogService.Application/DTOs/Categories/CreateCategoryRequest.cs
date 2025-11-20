namespace CatalogService.Application.DTOs.Categories;
public sealed record CreateCategoryRequest(
    string Name,
    string Slug,
    string? Description,
    Guid? ParentId
    );
