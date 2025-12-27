using CatalogService.Application.DTOs.Attributes;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.VariantAttributes;
using CatalogService.Infrastructure.Search.Elasticsearch.Mappings;
using CatalogService.Infrastructure.Search.ElasticSearch;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;

public sealed class ElasticsearchIndexManager(
    ElasticsearchClient client,
    IOptions<ElasticsearchSettings> options,
    ILogger<ElasticsearchIndexManager> logger) : IElasticsearchIndexManager
{
    private readonly ElasticsearchSettings _options = options.Value;
    
    public async Task<bool> CreateProductIndexAsync(CancellationToken ct = default)
    {
        var indexName = $"{_options.DefaultIndex}-{ElasticsearchIndexNames.ProductPostfixIndex}";

        if (await IndexExistsAsync(indexName, ct))
        {
            logger.LogInformation("Index {IndexName} already exists", indexName);
            return true;
        }


        var response = await client.Indices.CreateAsync<ProductDetailedResponse>(
            indexName,
            c => c
                .Settings(s => s
                    .NumberOfShards(_options.NumberOfShards)
                    .NumberOfReplicas(_options.NumberOfReplicas)
                    .Analysis(a => a
                        .Analyzers(an => an
                            .Custom("autocomplete", ca => ca
                                .Tokenizer("standard")
                                .Filter("lowercase", "autocomplete_filter")
                            )
                        )
                        .TokenFilters(tf => tf
                            .EdgeNGram("autocomplete_filter", e => e
                                .MinGram(2)
                                .MaxGram(20)
                            )
                        )
                    )
                )
                .Mappings(m => m.Properties(ProductMappings.PropertiyMappings)), ct);

        if (!response.IsValidResponse)
        {
            logger.LogError(
                "Failed to create product index: {Error}", 
                response.ElasticsearchServerError?.Error);
            return false;
        }

        logger.LogInformation(
            "Successfully created product index: {IndexName}", 
            indexName);

        return true;
    }

    public async Task<bool> CreateCategoryIndexAsync(CancellationToken ct = default)
    {
        var indexName = $"{_options.DefaultIndex}-{ElasticsearchIndexNames.CategoryPostfixIndex}";

        if (await IndexExistsAsync(indexName, ct))
        {
            logger.LogInformation("Index {IndexName} already exists", indexName);
            return true;
        }
        var response = await client.Indices.CreateAsync<CategoryDetailedResponse>(indexName, c => c
            .Settings(s => s
                .NumberOfShards(_options.NumberOfShards)
                .NumberOfReplicas(_options.NumberOfReplicas)
            )
            .Mappings(m => m
                .Properties(CategoryMappings.PropertiesMapping)), ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to create category index: {Error}", response.ElasticsearchServerError?.Error);
            return false;
        }

        logger.LogInformation("Successfully created category index: {IndexName}", indexName);
        return true;
    }

    public async Task<bool> CreateAttributeIndexAsync(CancellationToken ct = default)
    {
        var indexName = $"{_options.DefaultIndex}-{ElasticsearchIndexNames.AttributePostfixIndex}";

        if (await IndexExistsAsync(indexName, ct))
        {
            logger.LogInformation("Index {IndexName} already exists", indexName);
            return true;
        }
        var response = await client.Indices.CreateAsync<AttributeDetailedResponse>(indexName, c => c
            .Settings(s => s
                .NumberOfShards(_options.NumberOfShards)
                .NumberOfReplicas(_options.NumberOfReplicas)
            )
            .Mappings(p => p.Properties(at => at
                .Keyword(k => k.Id)
                .Text(t => t.Name, td => td.Fields(f => f.Keyword("keyword")))
                .Keyword(k => k.Code)
                .Keyword(k => k.OptionsType)
                .Boolean(b => b.IsFilterable)
                .Boolean(b => b.IsSearchable)
                .Boolean(b => b.IsActive)
                .Object(e => e.Options, o => o
                    .Properties(op => op
                        .Keyword(k => k.Options!.Values)
                    )
                )
                .Date(d => d.CreatedAt)
                )), ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to create attribute index: {Error}", response.ElasticsearchServerError?.Error);
            return false;
        }

        logger.LogInformation("Successfully created attribute index: {IndexName}", indexName);
        return true;
    }

    public async Task<bool> CreateVariantAttributeIndexAsync(CancellationToken ct = default)
    {
        var indexName = $"{_options.DefaultIndex}-{ElasticsearchIndexNames.VariantAttributeDefinitionPostfixIndex}";

        if (await IndexExistsAsync(indexName, ct))
        {
            logger.LogInformation("Index {IndexName} already exists", indexName);
            return true;
        }

        var response = await client.Indices.CreateAsync<VariantAttributeResponse>(indexName, c => c
            .Settings(s => s
                .NumberOfShards(_options.NumberOfShards)
                .NumberOfReplicas(_options.NumberOfReplicas)
            )
            .Mappings(m => m
                .Properties(vt => vt
                .Keyword(k => k.Id)
                .Keyword(k => k.Code)
                .Text(t => t.Name, td => td.Fields(f => f.Keyword("keyword")))
                .Keyword(k => k.Datatype)
                .Boolean(b => b.AffectsInventory)
                .Object(e => e.AllowedValues, o => o
                    .Properties(op => op
                        .Keyword(e => e.AllowedValues!.Values)
                    )
                )
                ))
            , ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to create variant attribute index: {Error}", response.ElasticsearchServerError?.Error);
            return false;
        }

        logger.LogInformation("Successfully created variant attribute index: {IndexName}", indexName);
        return true;
    }

    public async Task<bool> DeleteIndexAsync(string indexName, CancellationToken ct = default)
    {
        var response = await client.Indices.DeleteAsync(indexName, ct);
        return response.IsValidResponse;
    }

    public async Task<bool> IndexExistsAsync(string indexName, CancellationToken ct = default)
    {
        var response = await client.Indices.ExistsAsync(indexName, ct);
        return response.Exists;
    }

    public async Task<bool> ReindexAsync(string sourceIndex, string destinationIndex, CancellationToken ct = default)
    {
        var response = await client.ReindexAsync<dynamic>(r => r
            .Source(s => s.Indices(sourceIndex))
            .Dest(d => d.Index(destinationIndex))
            .WaitForCompletion(false)
        , ct);

        if (!response.IsValidResponse)
        {
            logger.LogError("Failed to reindex: {Error}", response.ElasticsearchServerError?.Error);
            return false;
        }

        logger.LogInformation("Reindex task started from {Source} to {Destination}", sourceIndex, destinationIndex);
        return true;
    }
}
