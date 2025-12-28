using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.BulkIndex.Commands.Attributes;

public sealed record ReindexAllAttributesCommand : ICommand;

internal sealed class ReindexAllAttributesCommandHandler(
    IBulkReindexService bulkReindexService,
    ILogger<ReindexAllAttributesCommandHandler> logger) : ICommandHandler<ReindexAllAttributesCommand>
{
    public async Task<Result> HandleAsync(ReindexAllAttributesCommand command, CancellationToken ct = default)
    {
        try
        {
            await bulkReindexService.ReindexAllAttributesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to reindex all attributes");
            return AttributeErrors.ReindexAttributes;
        }
    }
}