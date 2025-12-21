using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDeactivatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductDeactivatedDomainEventHandler> logger)
    :    ProductDomainEventHandlerBase(productSearchService, productQueries, logger),
         IDomainEventHandler<ProductDeactivatedDomainEvent>
{
    public async Task HandleAsync(ProductDeactivatedDomainEvent domainEvent, CancellationToken ct)
        => await base.HandleAsync(domainEvent.Id, ct);
}
