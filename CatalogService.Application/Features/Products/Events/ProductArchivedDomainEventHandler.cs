using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductArchivedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductArchivedDomainEventHandler> logger) 
    :   ProductDomainEventHandlerBase(productSearchService, productQueries, logger),
        IDomainEventHandler<ProductArchivedDomainEvent>
{
    public async Task HandleAsync(ProductArchivedDomainEvent domainEvent, CancellationToken cancellationToken)
        => await base.HandleAsync(domainEvent.Id, cancellationToken);
}
