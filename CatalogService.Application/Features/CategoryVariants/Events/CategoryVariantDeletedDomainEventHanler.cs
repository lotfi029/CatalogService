using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

namespace CatalogService.Application.Features.CategoryVariants.Events;

internal sealed class CategoryVariantDeletedDomainEventHanler(ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryVariantDeletedDomainEventHanler> logger)
    : CategoryVariantDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryVariantDeletedDomainEvent>
{
    public async Task HandleAsync(CategoryVariantDeletedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category variant deleted domain event with categoryId: {categoryId}",
                domainEvent.Id);
            return;
        }
    }
}