using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.BulkIndex.Commands.All;

public sealed record ReindexAllCommand : ICommand;

internal sealed class ReindexAllCommandHandler(
    IBulkReindexService bulkReindexService,
    ILogger<ReindexAllCommandHandler> logger) : ICommandHandler<ReindexAllCommand>
{
    public async Task<Result> HandleAsync(ReindexAllCommand command, CancellationToken ct = default)
    {
        try
        {
            await bulkReindexService.ReindexAllAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to reindex all");
            return Error.Unexpected("Failed to reindex all");
        }
    }
}