using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.Categories.Commands.AddVariant;

public sealed record AddCategoryVariantCommand(Guid Id, Guid VariantId, short DisplayOrder, bool IsRequired) : ICommand;

public sealed class AddCategoryVariantCommandHandler(
    ILogger<AddCategoryVariantCommandHandler> logger,
    ICategoryDomainService categoryDomainService,
    IVariantAttributeRepository variantRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddCategoryVariantCommand>
{
    public async Task<Result> HandleAsync(AddCategoryVariantCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty || command.VariantId == Guid.Empty)
            return CategoryErrors.InvalidId;

        if (await categoryRepository.FindByIdAsync(command.Id, ct) is not { } category)
            return CategoryErrors.NotFound(command.Id);

        if (!await variantRepository.ExistsAsync(command.VariantId, ct))
            return VariantAttributeErrors.NotFound(command.VariantId);

        try
        {
            var addingResult = await categoryDomainService.AddVariantAttributeToCategoryAsync(
                category,
                variantId: command.VariantId,
                displayOrder: command.DisplayOrder,
                isRequired: command.IsRequired,
                ct);

            if (addingResult.IsFailure)
                return addingResult.Error;

            categoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while adding variant attribute {VariantAttributeId} to category {CategoryId}",
                command.VariantId, command.Id);

            return Error.Unexpected("Error ocurred while adding an variant to category");
        }

    }
}