using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Events;

internal sealed class ProductVariantDeletedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductVariantDeletedDomainEventHandler> logger) 
    : ProductVariantDomainEventHandlerBase(productQueries, productSearchService, logger),
      IDomainEventHandler<ProductVariantDeletedDomainEvent>
{
    public async Task HandleAsync(ProductVariantDeletedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handle event product variant deleted with productId: {productId}, and variantId {variantId}",
                domainEvent.Id, domainEvent.VariantId);
        }
    }
}
