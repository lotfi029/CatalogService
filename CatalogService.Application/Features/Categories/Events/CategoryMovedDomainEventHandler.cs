using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryMovedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryMovedDomainEventHandler> logger)
    : CategoryDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryMovedDomainEvent>
{
    public async Task HandleAsync(CategoryMovedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category moved domain event");
        }
    }
}