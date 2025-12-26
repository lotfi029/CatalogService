using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.ElasticSearch;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;

internal sealed class CategorySearchService (
    ElasticsearchClient client,
    IOptions<ElasticsearchSettings> settings,
    ILogger<CategorySearchService> logger)
    : ElasticsearchService<CategoryDetailedResponse>(client ?? default!, ElasticsearchIndexNames.CategoryPostfixIndex, settings.Value.DefaultIndex, logger), 
    ICategorySearchService
{
    private readonly string _indexName = $"{settings.Value.DefaultIndex}-{ElasticsearchIndexNames.CategoryPostfixIndex}";

    public Task<List<CategoryDetailedResponse>> GetCategoryTreeAsync(Guid? parentId = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Task<(List<CategoryDetailedResponse> Categories, long Total)> SearchCategoriesAsync(string? searchTerm = null, Guid? parentId = null, bool? isActive = null, int from = 0, int size = 20, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}