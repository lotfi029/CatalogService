using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.Patch;

public sealed record PatchProductCategoryCommand(Guid ProductId, Guid CategoryId, bool IsPrimary) : ICommand;

internal sealed class PatchProductCategoryCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<PatchProductCategoryCommandHandler> logger) : ICommandHandler<PatchProductCategoryCommand>
{
    public async Task<Result> HandleAsync(PatchProductCategoryCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.CategoryId == Guid.Empty)
            return ProductCategoriesErrors.InvalidId;

        try
        {
            var updatingResult = await productService.UpdateProductCategoryAsync(
                productId: command.ProductId, 
                categoryId: command.CategoryId, 
                isPrimary: command.IsPrimary, 
                ct);

            if (updatingResult.IsFailure)
                return updatingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while update category: '{categoryId}' relation with product: '{productId}'",
                command.CategoryId, command.ProductId);

            return ProductCategoriesErrors.UpdateProductCategory;
        }
    }
}