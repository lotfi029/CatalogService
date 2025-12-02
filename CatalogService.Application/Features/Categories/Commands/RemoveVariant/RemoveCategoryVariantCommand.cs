using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.Categories.Commands.RemoveVariant;

public sealed record RemoveCategoryVariantCommand(
    Guid Id,
    Guid VariantId
) : ICommand;

internal sealed class RemoveCategoryVariantCommandHandler(
    ICategoryDomainService categoryDomainService,
    IUnitOfWork unitOfWork,
    ILogger<RemoveCategoryVariantCommandHandler> logger) : ICommandHandler<RemoveCategoryVariantCommand>
{
    public async Task<Result> HandleAsync(
        RemoveCategoryVariantCommand command,
        CancellationToken ct = default)
    {
        try
        {
            var result = await categoryDomainService.RemoveVariantAttributeFromCategoryAsync(
                command.Id,
                command.VariantId,
                ct);

            if (result.IsFailure)
                return result;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while removing variant attribute {VariantAttributeId} from category {CategoryId}",
                command.VariantId, command.Id);
            return Error.Unexpected("Failed to remove variant attribute from category");
        }
    }
}