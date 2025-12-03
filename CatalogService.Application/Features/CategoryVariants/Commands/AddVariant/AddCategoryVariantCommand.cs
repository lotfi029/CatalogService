using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.CategoryVariants.Commands.AddVariant;

public sealed record AddCategoryVariantCommand(Guid Id, Guid VariantId, short DisplayOrder, bool IsRequired) : ICommand;

public sealed class AddCategoryVariantCommandHandler(
    ILogger<AddCategoryVariantCommandHandler> logger,
    ICategoryDomainService categoryDomainService,
    IUnitOfWork unitOfWork) : ICommandHandler<AddCategoryVariantCommand>
{
    public async Task<Result> HandleAsync(AddCategoryVariantCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty || command.VariantId == Guid.Empty)
            return CategoryErrors.InvalidId;
        try
        {
            var addingResult = await categoryDomainService.AddVariantAttributeToCategoryAsync(
                command.Id,
                variantId: command.VariantId,
                displayOrder: command.DisplayOrder,
                isRequired: command.IsRequired,
                ct);

            if (addingResult.IsFailure)
                return addingResult;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while adding variant attribute {VariantAttributeId} to category {CategoryId}",
                command.VariantId, command.Id);

            return CategoryVariantAttributeErrors.AddCategoryVariantAttribute;
        }
    }
}