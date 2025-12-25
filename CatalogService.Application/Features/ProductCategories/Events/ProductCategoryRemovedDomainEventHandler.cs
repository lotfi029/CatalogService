using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Events;

internal sealed class ProductCategoryRemovedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductCategoryRemovedDomainEventHandler> logger) 
    :   ProductCategoryDomainEventHandlerBase(productQueries, productSearchService, logger), 
        IDomainEventHandler<ProductCategoryRemovedDomainEvent>
{
    public async Task HandleAsync(ProductCategoryRemovedDomainEvent domainEvent, CancellationToken ct)
    {

        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, 
                "Error ocurred while handling product category deleted, productId: {productId}, categoryId: {categoryId}",
                domainEvent.Id, domainEvent.CategoryId);
            return;
        }
    }
}
