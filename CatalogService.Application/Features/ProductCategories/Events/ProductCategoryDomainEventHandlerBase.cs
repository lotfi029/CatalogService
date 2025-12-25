using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.ProductCategories.Events;

internal abstract class ProductCategoryDomainEventHandlerBase(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger logger)
{
    protected async Task HandleAsync(Guid productId, CancellationToken ct = default)
    {
        var productResult = await productQueries.GetAsync(productId, ct);
        if (productResult.IsFailure)
        {
            logger.LogError(
                "Error ocurred while retrieve product query the error: {error}",
                productResult.Error.ToString());
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
        return;
    }
}
