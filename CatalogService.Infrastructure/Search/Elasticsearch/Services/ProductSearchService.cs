using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.ElasticSearch;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatalogService.Infrastructure.Search.Elasticsearch.Services;
internal sealed class ProductSearchService(
    ElasticsearchClient client,
    IOptions<ElasticsearchSettings> settings,
    ILogger<ProductSearchService> logger) 
    : ElasticsearchService<ProductDetailedResponse>(client ?? default!, ElasticsearchIndexNames.ProductPostfixIndex, settings.Value.DefaultIndex, logger), 
    IProductSearchService
{
    private readonly string _indexName = $"{settings.Value.DefaultIndex}-{ElasticsearchIndexNames.ProductPostfixIndex}";
    public Task<Dictionary<string, List<(string Value, long Count)>>> GetFacetsAsync(List<Guid>? categoryIds = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>> GetSuggestionsAsync(string prefix, int size = 10, CancellationToken ct = default)
    {
        var response = await client.SearchAsync<ProductDetailedResponse>(s => s
            .Indices(_indexName)
            .Size(size)
            .Query(q => q
                .Bool(b => b
                    .Should(
                        sh => sh.MatchPhrasePrefix(m => m.Field("name").Query(prefix).Boost(2)),
                        sh => sh.Prefix(m => m.Field("name.keyword").Value(prefix)
                    ))
                    .Filter(f => f.Term(t => t.Field("isActive").Value(true)))
                )
            )
            .Source(src => src.Filter(f => f.Includes(e => e.Name)))
            , ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Suggestions faileds: {Errors}", response.ElasticsearchServerError?.Error);
            return [];
        } 
        return response.Documents.Select(d => d.Name).Distinct().ToList();
    }

    public async Task<(List<ProductDetailedResponse> Products, long Total)> SearchProductsAsync(
        string? searchTerm = null,
        List<Guid>? categoryIds = null,
        Dictionary<string, List<string>>? filters = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int from = 0,
        int size = 20,
        CancellationToken ct = default)
    {
        try
        {
            var mustQueries = new List<Query>();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                mustQueries.Add(new MultiMatchQuery
                {
                    Fields = new[] { "name", "description", "productAttributes.value" },
                    Query = searchTerm,
                    Fuzziness = new Fuzziness("AUTO"),
                    Operator = Operator.Or
                });
            }

            if (categoryIds?.Count > 0)
            {
                mustQueries.Add(new TermsQuery
                {
                    Field = "productCategories.categoryId",
                    Terms = new TermsQueryField([.. categoryIds.Select(e => FieldValue.String(e.ToString()))])
                });
            }
            if (minPrice.HasValue || maxPrice.HasValue)
            {
                var rangeQuery = new NumberRangeQuery(new Field("variants.price"))
                {
                    Gte = (double?)minPrice ?? null,
                    Lte = (double?)maxPrice ?? null
                };
            }
            if (filters?.Count > 0)
            {
                foreach (var filter in filters)
                {
                    mustQueries.Add(new NestedQuery
                    {
                        Path = "productAttributes",
                        Query = new BoolQuery
                        {
                            Must =
                            [
                                new TermQuery
                                {
                                    Field = "attributes.attributeCode.keyword",
                                    Value = FieldValue.String(filter.Key)
                                },
                                new TermsQuery
                                {
                                    Field = "attributes.value.keyword",
                                    Terms = new TermsQueryField([.. filter.Value.Select(FieldValue.String)])
                                }
                            ]
                        }
                    });
                }
            }
            var searchResponse = await client.SearchAsync<ProductDetailedResponse>(s => s
            .Indices(_indexName)
            .From(from)
            .Size(size)
            .Query(q => q
                .Bool(b => b
                    .Must(mustQueries.ToArray())
                    .Filter(f => f.Term(t => t.Field("isActive").Value(true)))
                )
            )
            .Sort(sort => sort
                .Score(new ScoreSort { Order = SortOrder.Desc })
                .Field(f => f
                    .Field("createdAt")
                    .Order(SortOrder.Desc)
                )
            ), ct);

            if (searchResponse.IsValidResponse)
                return (searchResponse.Documents.ToList(), searchResponse.Total);

            return ([], 0);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while searching products.");
            return (new List<ProductDetailedResponse>(), 0);
        }
    }
}
