using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.CategoryVariants.Commands.UpdateVariant;

public sealed record UpdateCategoryVariantCommand(
    Guid Id,
    Guid VariantId,
    bool IsRequired,
    short DisplayOrder
) : ICommand;

internal sealed class UpdateCategoryVariantCommandHandler(
    ICategoryDomainService categoryDomainService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateCategoryVariantCommandHandler> logger) : ICommandHandler<UpdateCategoryVariantCommand>
{
    public async Task<Result> HandleAsync(
        UpdateCategoryVariantCommand command,
        CancellationToken ct = default)
    {
        try
        {
            var result = await categoryDomainService.UpdateCategoryVariantAttributeAsync(
                command.Id,
                command.VariantId,
                command.DisplayOrder,
                command.IsRequired,
                ct);

            if (result.IsFailure)
                return result;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while updating variant attribute {VariantAttributeId} for category {CategoryId}",
                command.VariantId, command.Id);
            return Error.Unexpected("Failed to update category variant attribute");
        }
    }
}