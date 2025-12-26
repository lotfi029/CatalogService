using CatalogService.Application.DTOs.Products;

namespace CatalogService.Application.Interfaces;

public interface IElasticsearchService<TDocument> where TDocument : class
{
    Task<Result> IndexDocumentAsync(Guid id, TDocument document, CancellationToken ct = default);
    Task<Result> IndexManyAsync(IEnumerable<TDocument> documents, CancellationToken ct = default);
    Task<Result> UpdateDocumentAsync(Guid id, TDocument document, CancellationToken ct = default);
    Task<Result> DeleteDocumentAsync(Guid id, CancellationToken ct = default);
    Task<Result<TDocument>> GetDocumentAsync(Guid id, CancellationToken ct = default);
    Task<Result> DocumentExistsAsync(Guid id, CancellationToken ct = default);
}
public interface IProductSearchService : IElasticsearchService<ProductDetailedResponse>
{
    Task<(List<ProductDetailedResponse> Products, long Total)> SearchProductsAsync(
        string? searchTerm = null,
        List<Guid>? categoryIds = null,
        Dictionary<string, List<string>>? filters = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? status = null,
        int from = 0,
        int size = 20,
        CancellationToken ct = default);

    Task<List<string>> GetSuggestionsAsync(string prefix, int size = 10, CancellationToken ct = default);

    Task<Dictionary<string, List<(string Value, long Count)>>> GetFacetsAsync(
        List<Guid>? categoryIds = null,
        CancellationToken ct = default);
}
