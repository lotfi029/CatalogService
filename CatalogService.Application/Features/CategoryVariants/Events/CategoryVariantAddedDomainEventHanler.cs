using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

namespace CatalogService.Application.Features.CategoryVariants.Events;

internal sealed class CategoryVariantAddedDomainEventHanler(ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryVariantAddedDomainEventHanler> logger)
    : CategoryVariantDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryVariantAddedDomainEvent>
{
    public async Task HandleAsync(CategoryVariantAddedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category variant added domain event with categoryId: {categoryId} and variantId: {variantId}",
                domainEvent.Id, domainEvent.VariantAttributeId);
            return;
        }
    }
}
