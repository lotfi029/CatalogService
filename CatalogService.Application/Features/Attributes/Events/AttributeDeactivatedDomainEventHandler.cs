using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Attributes;

namespace CatalogService.Application.Features.Attributes.Events;

internal sealed class AttributeDeactivatedDomainEventHandler(
    IAttributeQueries attributeQueries,
    IProductQueries productQueries,
    IProductAttributeRepository productAttributeRepository,
    IAttributeSearchService attributeSearchService,
    IProductSearchService productSearchService,
    ILogger<AttributeDeactivatedDomainEventHandler> logger)
    : AttributeIndexUpdatedWithProductsEventHandler(attributeQueries, attributeSearchService, productQueries, productAttributeRepository, productSearchService, logger),
    IDomainEventHandler<AttributeDeactivatedDomainEvent>
{
    public async Task HandleAsync(AttributeDeactivatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await UpdateAttributeIndexAsync(domainEvent.Id, ct);
            await UpdateRelatedProductIndexesAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute deactivated domain events with");
        }
    }
}
