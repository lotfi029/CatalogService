using CatalogService.Application.DTOs.CategoryVariantAttributes;
using CatalogService.Domain.DomainService.Categories;

namespace CatalogService.Application.Features.CategoryVariants.Commands.AddBulkVariants;

public sealed record AddCategoryVariantBulkCommand(
    Guid Id,
    AddCategoryVariantBulkRequest Request) : ICommand;

internal sealed class AddCategoryVariantBulkCommandHandler(
    ILogger<AddCategoryVariantBulkCommandHandler> logger,
    ICategoryDomainService categoryDomainService,
    IUnitOfWork unitOfWork) : ICommandHandler<AddCategoryVariantBulkCommand>
{
    public async Task<Result> HandleAsync(AddCategoryVariantBulkCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return CategoryErrors.InvalidId;
        try
        {
            var categoryVariant = command.Request.Variants.Select(e => (e.VariantId, e.IsRequired, e.DisplayOrder));
            var result = await categoryDomainService.AddBulkCategoryVariantAttributeAsync(
                id: command.Id,
                categoryVariant, ct);

            if (result.IsFailure)
                return result;

            var successAddedCatetoryVariant = await unitOfWork.SaveChangesAsync(ct);
            if (successAddedCatetoryVariant != categoryVariant.Count())
                return CategoryVariantAttributeErrors.FaildToAddVariantsToCategory(categoryVariant.Count());

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while trying to add bulk variant to category with id: '{categoryId}'",
                command.Id);

            return CategoryVariantAttributeErrors.AddCategoryVariantBulk;
        }
    }
}