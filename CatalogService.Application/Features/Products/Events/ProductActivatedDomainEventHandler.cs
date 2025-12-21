using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductActivatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQeuries,
    ILogger<ProductActivatedDomainEventHandler> logger) 
    :   ProductDomainEventHandlerBase(productSearchService, productQeuries, logger),
        IDomainEventHandler<ProductActivatedDomainEvent>
{
    public async Task HandleAsync(ProductActivatedDomainEvent domainEvent, CancellationToken cancellationToken)
        => await base.HandleAsync(domainEvent.Id, cancellationToken);
}
