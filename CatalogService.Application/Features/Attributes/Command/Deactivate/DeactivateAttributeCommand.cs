namespace CatalogService.Application.Features.Attributes.Command.Deactivate;

public sealed record DeactivateAttributeCommand(Guid Id) : ICommand;

internal sealed class DeactivateAttributeCommandHandler(
    IAttributeRepository attributeRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeactivateAttributeCommandHandler> logger) : ICommandHandler<DeactivateAttributeCommand>
{
    public async Task<Result> HandleAsync(DeactivateAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            if (await attributeRepository.FindByIdAsync(command.Id, ct) is not { } attribute)
                return AttributeErrors.NotFound(command.Id);

            if (attribute.Deactivate() is { IsFailure: true} error)
                return error;

            attributeRepository.Update(attribute);
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