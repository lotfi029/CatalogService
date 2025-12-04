namespace CatalogService.Application.Features.Attributes.Command.Delete;

public sealed record DeleteAttributeCommand(Guid Id) : ICommand;

internal sealed class DeleteAttributeCommandHandler(
    ILogger<DeleteAttributeCommandHandler> logger,
    IAttributeRepository attributeRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteAttributeCommand>
{
    public async Task<Result> HandleAsync(DeleteAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            if (await attributeRepository.FindByIdAsync(command.Id, ct) is not { } attribute)
                return AttributeErrors.NotFound(command.Id);

            if (attribute.Deleted() is { IsFailure: true } error)
                return error;
            attributeRepository.Update(attribute);

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while delete attribute with id {attributeId}",
                command.Id);
            return AttributeErrors.DeleteAttribute;
        }
    }
}