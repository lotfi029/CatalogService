using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Attributes;

namespace CatalogService.Application.Features.Attributes.Events;

internal sealed class AttributeCreatedDomainEventHandler(
    IAttributeQueries attributeQueries,
    IAttributeSearchService attributeSearchService,
    ILogger<AttributeCreatedDomainEventHandler> logger)
        : AttributeIndexingEventHandlerBase(attributeQueries, attributeSearchService, logger),
        IDomainEventHandler<AttributeCreatedDomainEvent>
{
    public async Task HandleAsync(AttributeCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await UpdateAttributeIndexAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute Created domain events with");
        }
    }
}
