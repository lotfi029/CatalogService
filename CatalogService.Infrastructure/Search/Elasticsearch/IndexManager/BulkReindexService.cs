using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;

internal sealed class BulkReindexService(
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
        var productQueries = scope.ServiceProvider.GetRequiredService<IProductQueries>();
        var esService = scope.ServiceProvider.GetRequiredService<IProductSearchService>();

        try
        {
            logger.LogInformation("Starting product reindex");

            var products = await productQueries.GetByIdsAsync(null, ct);
            if (products.Count == 0)
            {
                logger.LogInformation("There is no products to reindex");
                return;
            }
            logger.LogInformation("Found {Count} products to reindex", products.Count);

            var batches = products.Chunk(BatchSize);
            var totalBatches = (int)Math.Ceiling((double)products.Count / BatchSize);
            var currentBatch = 0;

            foreach (var document in batches)
            {
                currentBatch++;

                var success = await esService.IndexManyAsync(document, ct);

                if (success.IsSuccess)
                {
                    logger.LogInformation("Indexed batch {Current}/{Total} ({Count} products)",
                        currentBatch, totalBatches, document.Count());
                }
                else
                {
                    logger.LogError("Failed to index batch {Current}/{Total}", currentBatch, totalBatches);
                }

                await Task.Delay(100, ct);
            }

            logger.LogInformation("Completed product reindex: {Count} products indexed", products.Count);
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
        var esService = scope.ServiceProvider.GetRequiredService<ICategorySearchService>();

        try
        {
            logger.LogInformation("Starting category reindex");

            var documents = await categoryQuery.GetCategoriesByIdAsync([], ct: ct);
            if (documents.Count == 0)
            {
                logger.LogInformation("There is no categories to reindex");
                return;
            }
            logger.LogInformation("Found {Count} categories to reindex", documents.Count);
            await esService.IndexManyAsync(documents, ct);
            logger.LogInformation("Completed category reindex: {Count} categories indexed", documents.Count);
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
            if (documents.Count == 0)
            {
                logger.LogInformation("There is no attribute to reindex");
                return;
            }
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