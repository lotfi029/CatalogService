using CatalogService.Application.DTOs.Categories;

namespace CatalogService.Application.Interfaces;

public interface ICategorySearchService : IElasticsearchService<CategoryDetailedResponse>
{

    Task<(List<CategoryDetailedResponse> Categories, long Total)> SearchCategoriesAsync(
        string? searchTerm = null,
        Guid? parentId = null,
        bool? isActive = null,
        int from = 0,
        int size = 20,
        CancellationToken ct = default);

    Task<List<CategoryDetailedResponse>> GetCategoryTreeAsync(Guid? parentId = null, CancellationToken ct = default);
}
