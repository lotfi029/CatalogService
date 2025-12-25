using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductAttributes;

namespace CatalogService.Application.Features.ProductAttributes.Events;

internal sealed class ProductAttributeDeletedAllDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeDeletedAllDomainEventHandler> logger)
        : ProductAttributeDomainEventHandlerBase(productSearchService, productQueries, logger),
      IDomainEventHandler<ProductAttributeDeletedAllDomainEvent>
{
    public async Task HandleAsync(ProductAttributeDeletedAllDomainEvent domainEvent, CancellationToken ct)
        => await HandleAsync(domainEvent.Id, ct);
}