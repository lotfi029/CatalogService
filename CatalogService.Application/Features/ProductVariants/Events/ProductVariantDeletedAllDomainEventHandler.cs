using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductVariants;

namespace CatalogService.Application.Features.ProductVariants.Events;

internal sealed class ProductVariantDeletedAllDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductVariantDeletedAllDomainEventHandler> logger) 
    : ProductVariantDomainEventHandlerBase(productQueries, productSearchService, logger),
      IDomainEventHandler<ProductVariantDeletedAllDomainEvent>
{
    public async Task HandleAsync(ProductVariantDeletedAllDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handle event product variant deleted all with productId: {productId}",
                domainEvent.Id);
        }
    }
}