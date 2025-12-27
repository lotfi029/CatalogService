using CatalogService.Application.Features.Attributes.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Attributes.Events;

internal abstract class AttributeIndexingEventHandlerBase(
    IAttributeQueries attributeQueries,
    IAttributeSearchService attributeSearchService,
    ILogger logger)
{
    protected async Task UpdateAttributeIndexAsync(Guid attributeId, CancellationToken ct)
    {
        var attributeResult = await attributeQueries.GetAsync(attributeId, ct);
        if (attributeResult.IsFailure)
        {
            logger.LogError(
                "AttributeIndexingFailed: Unable to retrieve attribute. AttributeId={AttributeId}",
                attributeId);
            return;
        }

        var indexResult = await attributeSearchService.UpdateDocumentAsync(
            attributeId,
            attributeResult.Value!,
            ct);

        if (indexResult.IsFailure)
        {
            logger.LogError(
                "AttributeIndexingFailed: Unable to update attribute index. AttributeId={AttributeId}",
                attributeId);
        }
    }
}
