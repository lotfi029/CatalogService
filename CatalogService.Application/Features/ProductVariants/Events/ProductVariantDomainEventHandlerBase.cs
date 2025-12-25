using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.ProductVariants.Events;

internal abstract class ProductVariantDomainEventHandlerBase(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger logger)
{
    protected async Task HandleAsync(Guid productId, CancellationToken ct = default)
    {
        if (await productQueries.GetAsync(productId, ct) is not { IsFailure: true} product)
        {
            logger.LogError(
                "Error ocurred while retrieve product with Id: {productId}",
                productId);
            return; 
        }
        var updateDocumentResult = await productSearchService.UpdateDocumentAsync(
            productId, 
            product.Value!, 
            ct);

        if (updateDocumentResult.IsFailure)
        {
            logger.LogError(
                "Error ocurred while update products document with id: {productId}",
                productId);
        }

        return;
    }
}