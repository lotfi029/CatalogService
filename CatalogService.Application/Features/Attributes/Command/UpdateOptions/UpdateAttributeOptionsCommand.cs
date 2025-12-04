using CatalogService.Domain.DomainService.Attributes;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.Features.Attributes.Command.UpdateOptions;

public sealed record UpdateAttributeOptionsCommand(Guid Id, ValuesJson Options) : ICommand;

internal sealed class UpdateAttributeOptionsCommandHandler(
    ILogger<UpdateAttributeOptionsCommandHandler> logger,
    IAttributeDomainService attributeService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateAttributeOptionsCommand>
{
    public async Task<Result> HandleAsync(UpdateAttributeOptionsCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;
        try
        {
            var result = await attributeService.UpdateOptionsAsync(command.Id, command.Options, ct);
            if (result is { IsFailure: true })
                return result;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while update attribute options with id: {attributeId}",
                command.Id);
            return AttributeErrors.UpdateAttributeOptions;
        }
    }
}