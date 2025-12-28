using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;

public sealed class BulkReindexService(
    IServiceProvider serviceProvider,
    ILogger<BulkReindexService> logger) : IBulkReindexService
{
    private const int BatchSize = 500;

    public async Task ReindexAllAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Starting full reindex of all entities");

        await ReindexAllProductsAsync(ct);
        await ReindexAllCategoriesAsync(ct);
        await ReindexAllAttributesAsync(ct);

        logger.LogInformation("Completed full reindex of all entities");
    }

    public async Task ReindexAllProductsAsync(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var productQueries = scope.ServiceProvider.GetRequiredService<IProductQueries>();
        var esService = scope.ServiceProvider.GetRequiredService<IProductSearchService>();

        try
        {
            logger.LogInformation("Starting product reindex");

            var products = await productRepository.GetWithPredicateAsync(p => p.IsActive, ct);
            var productList = products.ToList();

            logger.LogInformation("Found {Count} products to reindex", productList.Count);

            var batches = productList.Chunk(BatchSize);
            var totalBatches = (int)Math.Ceiling((double)productList.Count / BatchSize);
            var currentBatch = 0;

            foreach (var batch in batches)
            {
                currentBatch++;
                var documents = await productQueries.GetByIdsAsync(productList.Select(e => e.Id), ct);

                var success = await esService.IndexManyAsync(documents, ct);

                if (success.IsSuccess)
                {
                    logger.LogInformation("Indexed batch {Current}/{Total} ({Count} products)",
                        currentBatch, totalBatches, documents.Count);
                }
                else
                {
                    logger.LogError("Failed to index batch {Current}/{Total}", currentBatch, totalBatches);
                }

                await Task.Delay(100, ct);
            }

            logger.LogInformation("Completed product reindex: {Count} products indexed", productList.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during product reindex");
            throw;
        }
    }

    public async Task ReindexAllCategoriesAsync(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var categoryQuery = scope.ServiceProvider.GetRequiredService<ICategoryQueries>();
        var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
        var esService = scope.ServiceProvider.GetRequiredService<ICategorySearchService>();

        try
        {
            logger.LogInformation("Starting category reindex");

            var categories = await categoryRepository.GetWithPredicateAsync(x => x.IsActive, ct);
            var documents = await categoryQuery.GetCategoriesByIdAsync([.. categories.Select(x => x.Id)], ct: ct);
            logger.LogInformation("Found {Count} categories to reindex", documents.Count);


            await esService.IndexManyAsync(documents, ct);

            logger.LogInformation("Completed category reindex: {Count} categories indexed", documents);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during category reindex");
            throw;
        }
    }

    public async Task ReindexAllAttributesAsync(CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        var attributeQuery = scope.ServiceProvider.GetRequiredService<IAttributeQueries>();
        var esService = scope.ServiceProvider.GetRequiredService<IAttributeSearchService>();

        try
        {
            logger.LogInformation("Starting attribute reindex");

            var documents = await attributeQuery.GetByIdsAsync(null, ct);

            await esService.IndexManyAsync(documents, ct);

            logger.LogInformation("Completed attribute reindex: {Count} attributes indexed", documents.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during attribute reindex");
            throw;
        }
    }
}