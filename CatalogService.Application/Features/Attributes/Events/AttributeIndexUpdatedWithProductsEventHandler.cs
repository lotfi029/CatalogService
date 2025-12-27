using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Attributes.Events;

internal abstract class AttributeIndexUpdatedWithProductsEventHandler(
    IAttributeQueries attributeQueries,
    IAttributeSearchService attributeSearchService,
    IProductQueries productQueries,
    IProductAttributeRepository productAttributeRepository,
    IProductSearchService productSearchService,
    ILogger logger)
        : AttributeIndexingEventHandlerBase(attributeQueries, attributeSearchService, logger ?? default!)
{
    protected async Task UpdateRelatedProductIndexesAsync(
        Guid attributeId,
        CancellationToken ct)
    {
        var productAttributes =
            await productAttributeRepository.GetAllByAttributeIdAsync(attributeId, [], ct);

        if (productAttributes is null || !productAttributes.Any())
            return;

        var tasks = productAttributes.Select(async pa =>
        {
            var productResult = await productQueries.GetAsync(pa.ProductId, ct);
            if (productResult.IsFailure)
            {
                logger.LogWarning(
                    "ProductReindexSkipped: Unable to retrieve product. ProductId={ProductId}",
                    pa.ProductId);
                return;
            }

            var indexResult = await productSearchService.UpdateDocumentAsync(
                pa.ProductId,
                productResult.Value!,
                ct);

            if (indexResult.IsFailure)
            {
                logger.LogWarning(
                    "ProductReindexFailed: Unable to update product index. ProductId={ProductId}",
                    pa.ProductId);
            }
        });

        await Task.WhenAll(tasks);
    }
}
