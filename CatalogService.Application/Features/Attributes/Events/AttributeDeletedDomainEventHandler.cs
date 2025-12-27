using CatalogService.Application.Features.Products.Queries;
using CatalogService.Application.Interfaces;
using CatalogService.Domain.Contants;
using CatalogService.Domain.DomainEvents.Attributes;

namespace CatalogService.Application.Features.Attributes.Events;

internal sealed class AttributeDeletedDomainEventHandler(
    IProductQueries productQueries,
    IProductAttributeRepository productAttributeRepository,
    IAttributeSearchService attributeSearchService,
    IProductSearchService productSearchService,
    ILogger<AttributeDeletedDomainEventHandler> logger)
    : IDomainEventHandler<AttributeDeletedDomainEvent>
{
    public async Task HandleAsync(AttributeDeletedDomainEvent domainEvent, CancellationToken ct)
    {
        try
        {
            if (await attributeSearchService.DeleteDocumentAsync(domainEvent.Id, ct) is { IsFailure: true } deleteDocumentErrors)
            {
                logger.LogError(
                    "AttributeDeleteDocumentFailed: unable to delete the attribute document. attributeId: '{attributeId}', error: '{errors}'",
                    domainEvent.Id, deleteDocumentErrors.Error.ToString());
                return;
            }
            var deletedProductAttributes = await productAttributeRepository
                .GetAllByAttributeIdAsync(domainEvent.Id, [QueryFilterConsts.SoftDeleteFilter], ct);
            
            if (deletedProductAttributes is null || !deletedProductAttributes.Any())
                return;

            var tasks = deletedProductAttributes.Select(async pa =>
            {
                var productResult = await productQueries.GetAsync(pa.ProductId, ct);
                if (productResult.IsFailure)
                {
                    logger.LogWarning(
                        "ProductReindexSkipped: Unable to retrieve product. ProductId={ProductId}",
                        pa.ProductId);
                    return;
                }

                var indexResult = await productSearchService.UpdateDocumentAsync(
                    pa.ProductId,
                    productResult.Value!,
                    ct);

                if (indexResult.IsFailure)
                {
                    logger.LogWarning(
                        "ProductReindexFailed: Unable to update product index. ProductId={ProductId}",
                        pa.ProductId);
                }
            });
            await Task.WhenAll(tasks);

        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while handling attribute deleted domain events with");
        }
    }
}