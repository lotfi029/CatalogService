using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Events;

internal sealed class ProductVariantUpdatedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductVariantUpdatedDomainEventHandler> logger) 
    : ProductVariantDomainEventHandlerBase(productQueries, productSearchService, logger),
      IDomainEventHandler<ProductVariantUpdatedDomainEvent>
{
    public async Task HandleAsync(ProductVariantUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handle event product variant updated with productId: {productId}, and variantId {variantId}",
                domainEvent.Id, domainEvent.VariantId);
        }
    }
}
