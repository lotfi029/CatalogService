using CatalogService.Domain.DomainService.Attributes;

namespace CatalogService.Application.Features.Attributes.Command.UpdateDetails;

public sealed class UpdateAttributeDetailsCommandHanlder(
    ILogger<UpdateAttributeDetailsCommandHanlder> logger,
    IAttributeDomainService attributeService,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateAttributeDetailsCommand>
{
    public async Task<Result> HandleAsync(UpdateAttributeDetailsCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            var result = await attributeService.UpdateDetailsAsync(
                id: command.Id,
                name: command.Name,
                isFilterable: command.IsFilterable,
                isSearchable: command.IsFilterable,
                ct: ct);

            if (result.IsFailure)
                return result.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while update attribute details with id: {attributeId}",
                command.Id);
            return AttributeErrors.UpdateAttributeDetails;
        }
    }
}