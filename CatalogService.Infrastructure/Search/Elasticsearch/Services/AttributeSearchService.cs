using CatalogService.Application.DTOs.Attributes;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.ElasticSearch;
using CatalogService.Infrastructure.Search.Errors;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;

internal sealed class AttributeSearchService(
    ElasticsearchClient client,
    IOptions<ElasticsearchSettings> settings,
    ILogger<AttributeSearchService> logger) 
    : ElasticsearchService<AttributeDetailedResponse>(client ?? default!, ElasticsearchIndexNames.AttributePostfixIndex, settings.Value.DefaultIndex, logger),
    IAttributeSearchService
{
    private string _indexName => $"{settings.Value.DefaultIndex}-{ElasticsearchIndexNames.AttributePostfixIndex}";
    public sealed override async Task<Result> IndexManyAsync(IEnumerable<AttributeDetailedResponse> documents, CancellationToken ct = default)
    {
        var response = await client.BulkAsync(b => b
            .Index(_indexName)
            .IndexMany(documents, (d, doc) => d.Id(doc.Id)),
            ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to bulk index documents: {Error}", response.ElasticsearchServerError?.Error);
            return ElasticsearchServiceErrors.IndexedFailed;
        }

        return Result.Success();
    }
}
