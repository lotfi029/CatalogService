using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Events;

internal sealed class ProductVariantAddedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductVariantAddedDomainEventHandler> logger) 
    : ProductVariantDomainEventHandlerBase(productQueries, productSearchService, logger),
      IDomainEventHandler<ProductVariantAddedDomainEvent>
{
    public async Task HandleAsync(ProductVariantAddedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handle event product variant added with productId: {productId}, and variantId {variantId}",
                domainEvent.Id, domainEvent.VariantId);
        }
    }
}
