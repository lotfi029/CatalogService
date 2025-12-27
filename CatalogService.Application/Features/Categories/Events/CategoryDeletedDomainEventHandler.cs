using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Contants;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryDeletedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    IProductCategoryRepository productCategoryRepository,
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<CategoryDeletedDomainEventHandler> logger)
    : CategoryIndexUpdateWithProductCategoriesEventHandlerBase(categoryQueries, categorySearchService, productCategoryRepository, productQueries, productSearchService, logger),
    IDomainEventHandler<CategoryDeletedDomainEvent>
{
    public async Task HandleAsync(CategoryDeletedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.UpdateCategoryIndexAsync(domainEvent.Id, ct);
            await base.UpdateRelatedProductsAsync(domainEvent.Id, [QueryFilterConsts.SoftDeleteFilter], ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category deleted domain event");
        }
    }
}