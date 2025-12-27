using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryDetailsUpdatedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    IProductCategoryRepository productCategoryRepository,
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<CategoryDetailsUpdatedDomainEventHandler> logger)
    : CategoryIndexUpdateWithProductCategoriesEventHandlerBase(categoryQueries, categorySearchService, productCategoryRepository, productQueries, productSearchService, logger),
    IDomainEventHandler<CategoryDetailsUpdatedDomainEvent>
{
    public async Task HandleAsync(CategoryDetailsUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.UpdateCategoryIndexAsync(domainEvent.Id, ct);
            await base.UpdateRelatedProductsAsync(domainEvent.Id, [], ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category details updated domain event");
        }
    }
}