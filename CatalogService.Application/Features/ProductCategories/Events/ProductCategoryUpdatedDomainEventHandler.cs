using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Events;

internal sealed class ProductCategoryUpdatedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductCategoryUpdatedDomainEventHandler> logger) 
    :   ProductCategoryDomainEventHandlerBase(productQueries, productSearchService, logger), 
        IDomainEventHandler<ProductCategoryUpdatedDomainEvent>
{
    public async Task HandleAsync(ProductCategoryUpdatedDomainEvent domainEvent, CancellationToken ct)
    {

        try
        {
            await base.HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, 
                "Error ocurred while handling product category updated, productId: {productId}, categoryId: {categoryId}",
                domainEvent.Id, domainEvent.CategoryId);
            return;
        }
    }
}
