namespace CatalogService.Application.Features.VariantAttributes.Commands.Delete;

internal sealed class DeleteVariantAttributeCommandHandler(
    ILogger<DeleteVariantAttributeCommandHandler> logger,
    IVariantAttributeRepository variantRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteVariantAttributeCommand>
{
    public async Task<Result> HandleAsync(DeleteVariantAttributeCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return VariantAttributeErrors.InvalidId;
        if (await variantRepository.FindByIdAsync(command.Id, ct) is not { } variantAttribute)
            return VariantAttributeErrors.NotFound(command.Id);

        try
        {
            variantAttribute.Delete();
            variantRepository.Update(variantAttribute);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex) 
        {
            logger.LogError(ex,
                "Error Ocurred while deleting an variant attribute definition with {id}",
                command.Id);

            return Error.Unexpected("Error Ocurred while deleting an variant attribute definition");
        }
    }
}