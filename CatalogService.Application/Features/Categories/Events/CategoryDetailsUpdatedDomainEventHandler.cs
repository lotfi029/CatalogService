using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryDetailsUpdatedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryDetailsUpdatedDomainEventHandler> logger)
    : CategoryDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryDetailsUpdatedDomainEvent>
{
    public async Task HandleAsync(CategoryDetailsUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category details updated domain event");
        }
    }
}