using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductCreatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductCreatedDomainEventHandler> logger) 
    :   ProductDomainEventHandlerBase(productSearchService, productQueries, logger),
        IDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task HandleAsync(ProductCreatedDomainEvent domainEvent, CancellationToken ct)
        => await base.HandleAsync(domainEvent.Id, ct);
}
