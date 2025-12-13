namespace CatalogService.Application.Features.VariantAttributes.Commands.Update;

internal sealed class UpdateVariantAttributeCommandHandler(
    ILogger<UpdateVariantAttributeCommandHandler> logger,
    IVariantAttributeRepository variantRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateVariantAttributeCommand>
{
    public async Task<Result> HandleAsync(UpdateVariantAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return VariantAttributeErrors.InvalidId;

        if (await variantRepository.FindAsync(command.Id,null, ct) is not { } variantAttribute)
            return VariantAttributeErrors.NotFound(command.Id);
        try
        {

            variantAttribute.Update(command.Request.Name, command.Request.AllowedValues);
            variantRepository.Update(variantAttribute);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, 
                "Error ocurred while update an variant attribute definition with id: '{id}'", 
                command.Id);

            return Error.Unexpected("Error ocurred while update an variant attribute definition");
        }
    }
}