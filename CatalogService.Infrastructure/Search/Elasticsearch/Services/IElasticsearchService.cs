using Result = SharedKernel.Result;


namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;

public interface IElasticsearchService<TDocument> where TDocument : class
{
    Task<Result> IndexDocumentAsync(TDocument document, CancellationToken ct = default);
    Task<Result> IndexManyAsync(IEnumerable<TDocument> documents, CancellationToken ct = default);
    Task<Result> UpdateDocumentAsync(string id, TDocument document, CancellationToken ct = default);
    Task<Result> DeleteDocumentAsync(string id, CancellationToken ct = default);
    Task<Result<TDocument>> GetDocumentAsync(string id, CancellationToken ct = default);
    Task<Result> DocumentExistsAsync(string id, CancellationToken ct = default);
}
