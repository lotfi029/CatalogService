using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products.ProductCategories;

namespace CatalogService.Application.Features.ProductCategories.Events;

internal sealed class ProductCategoryAddedDomainEventHandler(
    IProductQueries productQueries,
    IProductSearchService productSearchService,
    ILogger<ProductCategoryAddedDomainEventHandler> logger) 
    : ProductCategoryDomainEventHandlerBase(productQueries, productSearchService, logger),
    IDomainEventHandler<ProductCategoryAddedDomainEvent>
{
    public async Task HandleAsync(ProductCategoryAddedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            await HandleAsync(domainEvent.Id, ct);
            return;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, 
                "Error ocurred while handling product category added, productId: {productId}, categoryId: {categoryId}",
                domainEvent.Id, domainEvent.CategoryId);
            return;
        }
    }
}