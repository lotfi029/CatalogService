using CatalogService.Infrastructure.Search.Errors;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Result = SharedKernel.Result;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;

public sealed class ElasticsearchService<TDocument>(
    ElasticsearchClient client,
    string indexName,
    ILogger<ElasticsearchService<TDocument>> logger) : IElasticsearchService<TDocument>
    where TDocument : class
{
    public async Task<Result> IndexDocumentAsync(
        TDocument document,
        CancellationToken ct = default)
    {
        var response = await client.IndexAsync(document, indexName, ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to index document: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.IndexedFailed;
        }

        return Result.Success();
    }


    public async Task<Result> IndexManyAsync(IEnumerable<TDocument> documents, CancellationToken ct = default)
    {
        var response = await client.IndexManyAsync(documents, indexName, ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to bulk index documents: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.IndexedFailed;
        }

        return Result.Success();
    }

    public async Task<Result> UpdateDocumentAsync(string id, TDocument document, CancellationToken ct = default)
    {
        var response = await client.UpdateAsync<TDocument, TDocument>(
            indexName,
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

    public async Task<Result> DeleteDocumentAsync(string id, CancellationToken ct = default)
    {
        var response = await client.DeleteAsync(indexName, id, ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to delete document: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.DeletedFailed;
        }
        return Result.Success();
    }

    public async Task<Result<TDocument>> GetDocumentAsync(string id, CancellationToken ct = default)
    {
        var response = await client.GetAsync<TDocument>(indexName, id, ct);

        if (!response.IsValidResponse || !response.Found)
        {
            return ElasticsearchServiceErrors.NotFound;
        }

        return response.Source!;
    }

    public async Task<Result> DocumentExistsAsync(string id, CancellationToken ct = default)
    {
        var response = await client.ExistsAsync(indexName, id, ct);
        return response.Exists
            ? Result.Success()
            : ElasticsearchServiceErrors.NotFound;
    }
}