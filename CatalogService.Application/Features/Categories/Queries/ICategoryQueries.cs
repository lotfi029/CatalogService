using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Features.Categories.Queries;

public interface ICategoryQueries
{
    Task<Result<CategoryDetailedResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<CategoryDetailedResponse>> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Result<IEnumerable<CategoryResponse>>> GetTreeAsync(Guid? parentId, CancellationToken ct = default);
}
