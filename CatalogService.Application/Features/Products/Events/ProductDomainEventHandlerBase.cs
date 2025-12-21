using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Products.Events;

internal abstract class ProductDomainEventHandlerBase(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger logger)
{
    protected async Task HandleAsync(Guid productId, CancellationToken ct)
    {
        var productResult = await productQueries.GetAsync(productId, ct);

        if (productResult.IsFailure)
        {
            logger.LogError(
                "Failed to load product {ProductId}. Errors: {Errors}",
                productId,
                productResult.Error);
            return;
        }

        var updateResult = await productSearchService
            .UpdateDocumentAsync(productId, productResult.Value!, ct);

        if (updateResult.IsFailure)
        {
            logger.LogError(
                "Failed to update search document for ProductId {ProductId}. Errors: {Errors}",
                productId,
                updateResult.Error);
        }
    }
}
