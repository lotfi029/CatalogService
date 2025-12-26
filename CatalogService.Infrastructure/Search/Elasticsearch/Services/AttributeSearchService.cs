using CatalogService.Application.DTOs.Attributes;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.ElasticSearch;
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

}
