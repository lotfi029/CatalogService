using CatalogService.Application.Features.Categories.Queries;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.CategoryVariants.Events;

internal abstract class CategoryVariantDomainEventHandlerBase(
    ICategoryQueries categoryQueries,
    ICategorySearchService categorySearchService,
    ILogger logger)
{
    protected async Task HandleAsync(Guid id, CancellationToken ct = default)
    {
        if (await categoryQueries.GetDetailedCategoryResponse(id, ct) is not { IsSuccess: true } category)
        {
            logger.LogError(
                "Error ocurred while retrieve the category with id: {id} for documented it",
                id);
            return;
        }
        if (await categorySearchService.UpdateDocumentAsync(id, category.Value!, ct) is { IsFailure: true } categoriesError)
        {
            logger.LogError(
                "Error ocurred while document category with id: {id} with errors: {errors}",
                id, categoriesError.Error.ToString());
            return;
        }
    }
}
