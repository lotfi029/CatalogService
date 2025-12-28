using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.Errors;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;

internal abstract class ElasticsearchService<TDocument>(
    ElasticsearchClient client,
    string indexName,
    string defaultIndex,
    ILogger logger) : IElasticsearchService<TDocument>
    where TDocument : class
{

    private readonly string _indexName = $"{defaultIndex}-{indexName}";
    public async Task<Result> IndexDocumentAsync(
        Guid id,
        TDocument document,
        CancellationToken ct = default)
    {
        var response = await client.IndexAsync(document, _indexName, id, ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to index document: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.IndexedFailed;
        }

        return Result.Success();
    }

    public async Task<Result> UpdateDocumentAsync(
        Guid id,
        TDocument document,
        CancellationToken ct = default)
    {
        var response = await client.UpdateAsync<TDocument, TDocument>(
            _indexName,
            id,
            u => u.Doc(document).DocAsUpsert(true),
            ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to update document: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.UpdatedFailed;
        }

        return Result.Success();
    }

    public async Task<Result> DeleteDocumentAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await client.DeleteAsync(_indexName, id, ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to delete document: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.DeletedFailed;
        }
        return Result.Success();
    }

    public async Task<Result<TDocument>> GetDocumentAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await client.GetAsync<TDocument>(_indexName, id, ct);

        if (!response.IsValidResponse || !response.Found)
        {
            return ElasticsearchServiceErrors.NotFound;
        }

        return response.Source!;
    }

    public async Task<Result> DocumentExistsAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var response = await client.ExistsAsync(_indexName, id, ct);
        return response.Exists
            ? Result.Success()
            : ElasticsearchServiceErrors.NotFound;
    }

    public virtual Task<Result> IndexManyAsync(IEnumerable<TDocument> documents, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}