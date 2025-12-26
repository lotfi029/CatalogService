using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

namespace CatalogService.Application.Features.CategoryVariants.Events;

internal sealed class CategoryVariantUpdatedDomainEventHanler(ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger<CategoryVariantUpdatedDomainEventHanler> logger)
    : CategoryVariantDomainEventHandlerBase(categoryQueries, categorySearchService, logger),
    IDomainEventHandler<CategoryVariantUpdatedDomainEvent>
{
    public async Task HandleAsync(CategoryVariantUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category variant updated domain event with categoryId: {categoryId} and variantId: {variantId}",
                domainEvent.Id, domainEvent.VariantAttributeId);
            return;
        }
    }
}