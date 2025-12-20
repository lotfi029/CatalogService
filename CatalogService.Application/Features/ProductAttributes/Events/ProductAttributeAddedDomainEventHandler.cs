using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.ProductAttributes.Events;

internal class ProductAttributeAddedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductAttributeAddedDomainEventHandler> logger) : IDomainEventHandler<ProductAttributeAddedDomainEvent>
{
    public async Task HandleAsync(ProductAttributeAddedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            if (await productQueries.GetAsync(domainEvent.Id, ct) is not { IsFailure: true } product)
            {

            }
            if (await AddProductAttributeDocument(domainEvent.Id, product.Value, ct) is { IsFailure: true} result)
            {
                logger.LogError("Failed to update search document for ProductId: {ProductId}. Errors: {Errors}", domainEvent.Id, result.Errors);
            }
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Error handling ProductAttributeAddedDomainEvent for AttributeId: {AttributeId}", domainEvent.AttributeId);
        }
    }
    private async Task<Result> AddProductAttributeDocument(Guid productId, ProductDetailedResponse product, CancellationToken ct = default)
    { 
        return await productSearchService.UpdateDocumentAsync(productId, product, ct);
    }
}
