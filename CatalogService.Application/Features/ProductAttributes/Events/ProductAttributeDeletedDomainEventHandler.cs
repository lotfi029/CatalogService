using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.ProductAttributes.Events;

internal sealed class ProductAttributeDeletedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeDeletedDomainEventHandler> logger)
        : ProductAttributeDomainEventHandlerBase(productSearchService, productQueries, logger),
      IDomainEventHandler<ProductAttributeDeletedDomainEvent>
{
    public async Task HandleAsync(ProductAttributeDeletedDomainEvent domainEvent, CancellationToken ct)
        => await HandleAsync(domainEvent.Id, ct);
}
