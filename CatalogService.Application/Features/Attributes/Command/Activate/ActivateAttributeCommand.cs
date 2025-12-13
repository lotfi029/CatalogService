namespace CatalogService.Application.Features.Attributes.Command.Activate;

public sealed record ActivateAttributeCommand(Guid Id) : ICommand;

internal sealed class ActivateAttributeCommandHandler(
    IAttributeRepository attributeRepository,
    IUnitOfWork unitOfWork,
    ILogger<ActivateAttributeCommandHandler> logger) : ICommandHandler<ActivateAttributeCommand>
{
    public async Task<Result> HandleAsync(ActivateAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            if (await attributeRepository.FindAsync(command.Id, null, ct) is not { } attribute)
                return AttributeErrors.NotFound(command.Id);

            if (attribute.Activate() is { IsFailure: true } error)
                return error;

            attributeRepository.Update(attribute);
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