using CatalogService.Domain.DomainService.Attributes;

namespace CatalogService.Application.Features.Attributes.Command.Delete;

public sealed record DeleteAttributeCommand(Guid Id) : ICommand;

internal sealed class DeleteAttributeCommandHandler(
    ILogger<DeleteAttributeCommandHandler> logger,
    IAttributeDomainService attributeService,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteAttributeCommand>
{
    public async Task<Result> HandleAsync(DeleteAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            await attributeService.DeleteAsync(command.Id, ct);
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