using CatalogService.Application.DTOs.VariantAttributes;
using CatalogService.Domain.DomainService.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Commands.Create;

public sealed record CreateVariantAttributeCommand(CreateVariantAttributeRequest Request) : ICommand<Guid>;

internal sealed class CreateVariantAttributeCommandHandler(
    ILogger<CreateVariantAttributeCommandHandler> logger,
    IVariantAttributeDomainService variantService,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateVariantAttributeCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateVariantAttributeCommand command, CancellationToken ct = default)
    {
        try
        {
            var variantResult = await variantService.CreateAsync(
                code: command.Request.Code,
                name: command.Request.Name,
                datatype: command.Request.Datatype,
                affectsInventory: command.Request.AffectedInventory,
                allowedValues: command.Request.AllowedValues,
                ct);

            if (variantResult.IsFailure)
                return variantResult.Error;

            var variant = variantResult.Value!;

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success(variant.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Ocurred while adding new variant attribute definition");

            return VariantAttributeErrors.CreateCommandException;
        }
    }
}