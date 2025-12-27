using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Attributes;

namespace CatalogService.Application.Features.Attributes.Events;

internal sealed class AttributeOptionsUpdatedDomainEventHandler(
    IAttributeQueries attributeQueries,
    IProductQueries productQueries,
    IProductAttributeRepository productAttributeRepository,
    IAttributeSearchService attributeSearchService,
    IProductSearchService productSearchService,
    ILogger<AttributeOptionsUpdatedDomainEventHandler> logger)
    : AttributeIndexUpdatedWithProductsEventHandler(attributeQueries, attributeSearchService, productQueries, productAttributeRepository, productSearchService, logger),
    IDomainEventHandler<AttributeOptionsUpdatedDomainEvent>
{
    public async Task HandleAsync(AttributeOptionsUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await UpdateAttributeIndexAsync(domainEvent.Id, ct);
            await UpdateRelatedProductIndexesAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute options updated domain events with");
        }
    }
}
