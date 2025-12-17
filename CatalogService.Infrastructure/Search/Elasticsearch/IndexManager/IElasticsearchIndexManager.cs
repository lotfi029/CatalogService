namespace CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;

public interface IElasticsearchIndexManager
{
    Task<bool> CreateProductIndexAsync(CancellationToken ct = default);
    Task<bool> CreateCategoryIndexAsync(CancellationToken ct = default);
    Task<bool> CreateAttributeIndexAsync(CancellationToken ct = default);
    Task<bool> CreateVariantAttributeIndexAsync(CancellationToken ct = default);
    Task<bool> DeleteIndexAsync(string indexName, CancellationToken ct = default);
    Task<bool> IndexExistsAsync(string indexName, CancellationToken ct = default);
    Task<bool> ReindexAsync(string sourceIndex, string destinationIndex, CancellationToken ct = default);
}