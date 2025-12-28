using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.BulkIndex.Commands.Products;

public sealed record ReindexAllProductsCommand : ICommand;

internal sealed class ReindexAllProductsCommandHandler(
    IBulkReindexService bulkReindexService,
    ILogger<ReindexAllProductsCommandHandler> logger) : ICommandHandler<ReindexAllProductsCommand>
{
    public async Task<Result> HandleAsync(ReindexAllProductsCommand command, CancellationToken ct = default)
    {
        try
        {
            await bulkReindexService.ReindexAllProductsAsync(ct);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to reindex products");

            return ProductErrors.ReIndexProducts;
        }
    }
}