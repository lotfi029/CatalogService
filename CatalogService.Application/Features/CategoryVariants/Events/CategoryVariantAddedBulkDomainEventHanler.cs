using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

namespace CatalogService.Application.Features.CategoryVariants.Events;

internal sealed class CategoryVariantAddedBulkDomainEventHanler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryVariantAddedBulkDomainEventHanler> logger)
    : CategoryVariantDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryVariantAddedBulkDomainEvent>
{
    public async Task HandleAsync(CategoryVariantAddedBulkDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category variant added bulk domain event with categoryId: {categoryId}",
                domainEvent.Id);
            return;
        }
    }
}