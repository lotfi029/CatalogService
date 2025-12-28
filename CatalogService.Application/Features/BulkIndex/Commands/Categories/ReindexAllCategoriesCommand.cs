using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.BulkIndex.Commands.Categories;

public sealed record ReindexAllCategoriesCommand : ICommand;

internal sealed class ReindexAllCategoriesCommandHandler(
    IBulkReindexService bulkReindexService,
    ILogger<ReindexAllCategoriesCommandHandler> logger) : ICommandHandler<ReindexAllCategoriesCommand>
{
    public async Task<Result> HandleAsync(ReindexAllCategoriesCommand command, CancellationToken ct = default)
    {
        try
        {
            await bulkReindexService.ReindexAllCategoriesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to reindex all categories");

            return CategoryErrors.ReindexCategories;
        }

    }
}