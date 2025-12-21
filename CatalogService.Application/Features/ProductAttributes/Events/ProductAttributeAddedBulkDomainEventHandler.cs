using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.ProductAttributes.Events;

internal sealed class ProductAttributeAddedBulkDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeAddedBulkDomainEventHandler> logger)
        : ProductAttributeDomainEventHandlerBase(productSearchService, productQueries, logger),
      IDomainEventHandler<ProductAttributeAddedBulkDomainEvent>
{
    public async Task HandleAsync(ProductAttributeAddedBulkDomainEvent domainEvent, CancellationToken ct)
        => await HandleAsync(domainEvent.Id, ct);
}
