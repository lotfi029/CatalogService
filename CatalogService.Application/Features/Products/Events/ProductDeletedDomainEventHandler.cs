using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDeletedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductDeactivatedDomainEventHandler> logger) 
    :   ProductDomainEventHandlerBase(productSearchService, productQueries, logger),
        IDomainEventHandler<ProductDeletedDomainEvent>
{
    public async Task HandleAsync(ProductDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
        => await base.HandleAsync(domainEvent.Id, cancellationToken);
}