using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDetailsUpdatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductDetailsUpdatedDomainEventHandler> logger) 
    :   ProductDomainEventHandlerBase(productSearchService, productQueries, logger),
        IDomainEventHandler<ProductDetailsUpdatedDomainEvent>
{
    public async Task HandleAsync(ProductDetailsUpdatedDomainEvent domainEvent, CancellationToken ct)
        => await base.HandleAsync(domainEvent.Id, ct);
}

