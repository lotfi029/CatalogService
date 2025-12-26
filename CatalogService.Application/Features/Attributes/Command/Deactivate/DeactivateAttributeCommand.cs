using CatalogService.Domain.DomainService.Attributes;

namespace CatalogService.Application.Features.Attributes.Command.Deactivate;

public sealed record DeactivateAttributeCommand(Guid Id) : ICommand;

internal sealed class DeactivateAttributeCommandHandler(
    IAttributeDomainService attributeDomainService,
    IUnitOfWork unitOfWork,
    ILogger<DeactivateAttributeCommandHandler> logger) : ICommandHandler<DeactivateAttributeCommand>
{
    public async Task<Result> HandleAsync(DeactivateAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            if (await attributeDomainService.DeactiveAsync(command.Id, ct) is { IsFailure: true } deactivationErrors)
                return deactivationErrors;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deactivating attribute {AttributeId}", command.Id);
            return AttributeErrors.DeactivateAttribute;
        }
    }
}