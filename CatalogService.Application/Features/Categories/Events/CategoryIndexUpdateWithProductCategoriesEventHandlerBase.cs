using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Categories.Events;

internal abstract class CategoryIndexUpdateWithProductCategoriesEventHandlerBase(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    IProductCategoryRepository productCategoryRepository,
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger logger) : CategoryIndexingEventHandlerBase(categoryQueries, categorySearchService, logger ?? default!)
{
    protected async Task UpdateRelatedProductsAsync(Guid id, string[] ignoredQueryFilters, CancellationToken ct = default)
    {
        var productCategories = await productCategoryRepository
            .GetByCategoryIdAsync(id, ignoredQueryFilters, ct);

        if (productCategories is null || !productCategories.Any())
            return;

        var tasks = productCategories.Select(async pc =>
        {
            var productResult = await productQueries.GetAsync(pc.ProductId, ct);
            if (productResult.IsFailure)
            {
                logger.LogWarning(
                    "ProductReindexSkipped: Unable to retrieve product. ProductId={ProductId}",
                    pc.ProductId);
                return;
            }

            var indexResult = await productSearchService.UpdateDocumentAsync(
                pc.ProductId,
                productResult.Value!,
                ct);

            if (indexResult.IsFailure)
            {
                logger.LogWarning(
                    "ProductReindexFailed: Unable to update product index. ProductId={ProductId}",
                    pc.ProductId);
            }

        });

        await Task.WhenAll(tasks);
    }
}