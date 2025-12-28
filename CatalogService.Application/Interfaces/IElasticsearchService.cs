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
