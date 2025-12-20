using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDeactivatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductDeactivatedDomainEventHandler> logger) : IDomainEventHandler<ProductDeactivatedDomainEvent>
{
    public async Task HandleAsync(ProductDeactivatedDomainEvent domainEvent, CancellationToken ct)
    {
        if (await productQueries.GetAsync(domainEvent.Id, ct) is not { IsFailure: true } product)
        {
            logger.LogError("Product with Id: {ProductId} was not found in the database to deactivate it in search index.", domainEvent.Id);
            return;
        }
        if (await DeactiveDocument(domainEvent.Id, product.Value!, ct) is { IsFailure: true })
        {
            logger.LogError("Failed to delete product with Id: {ProductId} from search index.", domainEvent.Id);
            return;
        }

        logger.LogInformation("Product with Id: {ProductId} deleted successfully from search index.", domainEvent.Id);
    }
    private async Task<Result> DeactiveDocument(Guid productId, ProductDetailedResponse product, CancellationToken ct = default)
    {
        if (await productSearchService.UpdateDocumentAsync(productId, product, ct) is { IsFailure: true } result)
        {
            return result.Error;
        }
        
        return Result.Success();
    }
}
