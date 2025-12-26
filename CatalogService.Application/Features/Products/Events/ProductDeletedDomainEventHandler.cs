using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDeletedDomainEventHandler(
    IProductSearchService productSearchService,
    ILogger<ProductDeactivatedDomainEventHandler> logger) : IDomainEventHandler<ProductDeletedDomainEvent>
{
    public async Task HandleAsync(ProductDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            var deletingProductResult = await productSearchService.DeleteDocumentAsync(domainEvent.Id, cancellationToken);
            if (deletingProductResult.IsFailure)
            {
                logger.LogError("Error ocurred while deleting product document the error: {error}",
                    deletingProductResult.Error.ToString());
            }
            return;
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error Ocurred while handling product deleting event with product Id: {productId}",
                domainEvent.Id);
        }
    }
}