using CatalogService.Domain.DomainService.Attributes;

namespace CatalogService.Application.Features.Attributes.Command.Activate;

public sealed record ActivateAttributeCommand(Guid Id) : ICommand;

internal sealed class ActivateAttributeCommandHandler(
    IAttributeDomainService attributeDomainService,
    IUnitOfWork unitOfWork,
    ILogger<ActivateAttributeCommandHandler> logger) : ICommandHandler<ActivateAttributeCommand>
{
    public async Task<Result> HandleAsync(ActivateAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            if (await attributeDomainService.ActiveAsync(command.Id, ct) is { IsFailure: true } activationErrors)
                return activationErrors;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while activating attribute {AttributeId}", command.Id);
            return AttributeErrors.ActivateAttribute;
        }
    }
}