using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.ProductAttributes.Events;

internal sealed class ProductAttributeUpdatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeUpdatedDomainEventHandler> logger)
        : ProductAttributeDomainEventHandlerBase(productSearchService, productQueries, logger),
      IDomainEventHandler<ProductAttributeUpdatedDomainEvent>
{
    public async Task HandleAsync(ProductAttributeUpdatedDomainEvent domainEvent, CancellationToken ct)
        => await HandleAsync(domainEvent.Id, ct);
}
