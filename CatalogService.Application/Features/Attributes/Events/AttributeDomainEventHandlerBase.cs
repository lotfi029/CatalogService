using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Attributes;

namespace CatalogService.Application.Features.Attributes.Events;

internal abstract class AttributeDomainEventHandlerBase(
    IAttributeQueries attributeQueries,
    IProductQueries productQueries,
    IProductAttributeRepository productAttributeRepository,
    IAttributeSearchService attributeSearchService,
    IProductSearchService productSearchService,
    ILogger logger)
{
    protected async Task HandleAsync(Guid id, CancellationToken ct = default)
    {
        if (await attributeQueries.GetAsync(id, ct) is not { IsSuccess: true } attribute)
        {
            logger.LogError("error while retrieve attribute with id: {id}", id);
            return;
        }
        if (await attributeSearchService.UpdateDocumentAsync(id, attribute.Value!, ct) is { IsFailure: true })
        {
            logger.LogError("Error ocurred while document the index attributes with id: {id}", id);
            return;
        }
    }
    protected async Task UpdateProductAttributesAsync(Guid id, CancellationToken ct = default)
    {
        if (await productAttributeRepository.GetAllByAttributeIdAsync(id, ct) is not { } productAttributes)
            return;

        


    }
}

internal sealed class AttributeCreatedDomainEventHandler(
    IAttributeQueries attributeQueries,
    IAttributeSearchService attributeSearchService,
    ILogger<AttributeCreatedDomainEventHandler> logger)
    : AttributeDomainEventHandlerBase(attributeQueries, attributeSearchService, logger),
    IDomainEventHandler<AttributeCreatedDomainEvent>
{
    public async Task HandleAsync(AttributeCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute Created domain events with");
        }
    }
}
internal sealed class AttributeCreatedDomainEventHandler(
    IAttributeQueries attributeQueries,
    IAttributeSearchService attributeSearchService,
    ILogger<AttributeCreatedDomainEventHandler> logger)
    : AttributeDomainEventHandlerBase(attributeQueries, attributeSearchService, logger),
    IDomainEventHandler<AttributeCreatedDomainEvent>
{
    public async Task HandleAsync(AttributeCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute Created domain events with");
        }
    }
}