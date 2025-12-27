using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Categories;

namespace CatalogService.Application.Features.Categories.Events;

internal sealed class CategoryMovedDomainEventHandler(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    IProductCategoryRepository productCategoryRepository,
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<CategoryMovedDomainEventHandler> logger)
    : CategoryIndexUpdateWithProductCategoriesEventHandlerBase(categoryQueries, categorySearchService, productCategoryRepository, productQueries, productSearchService, logger),
    IDomainEventHandler<CategoryMovedDomainEvent>
{
    public async Task HandleAsync(CategoryMovedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await base.UpdateCategoryIndexAsync(domainEvent.Id, ct);
            await base.UpdateRelatedProductsAsync(domainEvent.Id, [], ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling category moved domain event");
        }
    }
}