using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductAttributes;

namespace CatalogService.Application.Features.ProductAttributes.Events;
internal sealed class ProductAttributeAddedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeAddedDomainEventHandler> logger)
        : ProductAttributeDomainEventHandlerBase(productSearchService, productQueries, logger),
      IDomainEventHandler<ProductAttributeAddedDomainEvent>
{
    public async Task HandleAsync(ProductAttributeAddedDomainEvent domainEvent, CancellationToken ct)
        => await HandleAsync(domainEvent.Id, ct);
}
