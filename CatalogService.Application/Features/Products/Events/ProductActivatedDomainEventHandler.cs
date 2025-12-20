using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductActivatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQeuries,
    ILogger<ProductActivatedDomainEventHandler> logger) : IDomainEventHandler<ProductActivatedDomainEvent>
{
    public async Task HandleAsync(ProductActivatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        if (await productQeuries.GetAsync(domainEvent.Id, ct: cancellationToken) is not { IsFailure: true } product)
        {
            logger.LogError("Product with Id: {ProductId} was not found in the database to update its index in search.", domainEvent.Id);
            return;
        }
        if (await ActiveDocument(product.Value!, cancellationToken) is { IsFailure: true })
        {
            logger.LogError("Failed to update index for product with Id: {ProductId} in search.", domainEvent.Id);
            return;
        }
        logger.LogInformation("Product with Id: {ProductId} index updated successfully in search.", domainEvent.Id);
    }
    private async Task<Result> ActiveDocument(ProductDetailedResponse product, CancellationToken ct = default)
    {
        await productSearchService.UpdateDocumentAsync(product.Id, product, ct);
        return Result.Success();
    }
}
