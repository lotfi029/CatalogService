using CatalogService.Application.DTOs.VariantAttributes;
using CatalogService.Domain.DomainService.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Commands.CreateBulk;

public sealed record CreateVariantAttributeBulkCommand(CreateVariantAttributeBulkRequest Request) : ICommand;

public sealed class CreateVariantAttributeBulkCommandHandler(
    ILogger<CreateVariantAttributeBulkCommandHandler> logger,
    IVariantAttributeDomainService variantService,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateVariantAttributeBulkCommand>
{
    public async Task<Result> HandleAsync(CreateVariantAttributeBulkCommand command, CancellationToken ct = default)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var variants = command.Request.Variants.Select(v => (v.Name, v.Code, v.Datatype, v.AffectedInventory, v.AllowedValues));

            var result = await variantService.CreateBulkAsync(variants, ct);
            if (result.IsFailure)
                return result;

            var successAddVariant = await unitOfWork.SaveChangesAsync(ct);
            if (successAddVariant != variants.Count())
            {
                await unitOfWork.RollBackTransactionAsync(transaction, ct);
                return VariantAttributeErrors.FailedToAddVariantAttribute(variants.Count());
            }
            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollBackTransactionAsync(transaction, ct);
            logger.LogError(ex, "Error ocurred while creating bulk of variant attribute definition");
            return VariantAttributeErrors.CreateBulkCommandException;
        }
    }
}