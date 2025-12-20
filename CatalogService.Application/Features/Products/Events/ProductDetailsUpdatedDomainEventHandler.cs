using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductDetailsUpdatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductDetailsUpdatedDomainEventHandler> logger) : IDomainEventHandler<ProductDetailsUpdatedDomainEvent>
{
    public async Task HandleAsync(ProductDetailsUpdatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            if (await productQueries.GetAsync(domainEvent.Id, ct: ct) is not { IsFailure: true } product)
            {
                logger.LogError("Product with Id: {ProductId} was not found in the database to index it in search.", domainEvent.Id);
                return;
            }
            if (await UpdateDocument(product.Value!, ct) is { IsFailure: true } result)
            {
                logger.LogError("Failed to update index for product with Id: {ProductId} in search.", domainEvent.Id);
                return;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private async Task<Result> UpdateDocument(ProductDetailedResponse product, CancellationToken ct = default)
    {
        await productSearchService.UpdateDocumentAsync(product.Id, product, ct);

        return Result.Success();
    }
}

