using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.DomainEvents.Products;

namespace CatalogService.Application.Features.Products.Events;

internal sealed class ProductCreatedDomainEventHandler(
    IProductSearchService productSearchService,
    IProductQueries productQueries,
    ILogger<ProductCreatedDomainEventHandler> logger) : IDomainEventHandler<ProductCreatedDomainEvent>
{
    public async Task HandleAsync(ProductCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            if (await productQueries.GetAsync(domainEvent.Id, ct: ct) is not { IsFailure: true } product)
            {
                logger.LogError("Product with Id: {ProductId} was not found in the database to index it in search.", domainEvent.Id);
                return;
            }

            if (await IndexDocument(product.Value!, ct) is { IsFailure: true })
            {
                logger.LogError("Failed to index product with Id: {ProductId} in search.", domainEvent.Id);
                return;
            }
            logger.LogInformation("Product with Id: {ProductId} indexed successfully in search.", domainEvent.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while indexing product with Id: {ProductId} in search.", domainEvent.Id);
        }
    }

    private async Task<Result> IndexDocument(ProductDetailedResponse product, CancellationToken ct = default)
    {
        await productSearchService.IndexDocumentAsync(product.Id, product, ct);

        return Result.Success();
    }
}
