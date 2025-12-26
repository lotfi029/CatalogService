using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryDeletedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryDeletedDomainEventHandler> logger)
    : CategoryDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryDeletedDomainEvent>
{
    public async Task HandleAsync(CategoryDeletedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category deleted domain event");
        }
    }
}