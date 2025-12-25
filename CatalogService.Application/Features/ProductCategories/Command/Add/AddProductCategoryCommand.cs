using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.AddCategory;

public sealed record AddProductCategoryCommand(Guid ProductId, Guid CategoryId, bool IsPrimary, List<ProductVariantRequest> Request) : ICommand;

internal sealed class AddProductCategoryCommandHandler(
    IProductDomainService productDomainService,
    IUnitOfWork unitOfWork,
    ILogger<AddProductCategoryCommandHandler> logger) : ICommandHandler<AddProductCategoryCommand>
{
    public async Task<Result> HandleAsync(AddProductCategoryCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.CategoryId == Guid.Empty)
            return ProductCategoriesErrors.InvalidId;

        var trainsaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var addingResult = await productDomainService.AddProductCategory(
                productId: command.ProductId,
                categoryId: command.CategoryId,
                isPrimary: command.IsPrimary,
                productVariants: [.. command.Request?.Select(pv => (pv.Price, pv.CompareAtPrice, pv.Variants)) ?? []],
                ct: ct);

            if (addingResult.IsFailure)
            {
                await unitOfWork.RollBackTransactionAsync(trainsaction, ct);
                return addingResult.Error;
            }

            await unitOfWork.SaveChangesAsync(ct);
            await unitOfWork.CommitTransactionAsync(trainsaction, ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            await unitOfWork.RollBackTransactionAsync(trainsaction, ct);
            logger.LogError(ex,
                "Failed To add category with id: '{categoryId}' to product with id: '{productId}'",
                command.CategoryId, command.ProductId);

            return ProductCategoriesErrors.AddProductCategory;
        }
    }
}