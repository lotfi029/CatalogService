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
            if (await productSearchService.DeleteDocumentAsync(domainEvent.Id, cancellationToken) is { IsFailure: true } result)
            {
                logger.LogError("Failed to delete product with Id: {ProductId} from search.", domainEvent.Id);
                return;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting product with Id: {ProductId} from search.", domainEvent.Id);
        }
    }
}