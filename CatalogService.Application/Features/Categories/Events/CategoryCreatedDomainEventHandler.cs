using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryCreatedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryCreatedDomainEventHandler> logger)
    : CategoryIndexingEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryCreatedDomainEvent>
{
    public async Task HandleAsync(CategoryCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.UpdateCategoryIndexAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category created domain event");
        }
    }
}