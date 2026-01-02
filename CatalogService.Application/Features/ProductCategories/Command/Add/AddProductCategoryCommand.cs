using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.Add;

public sealed record AddProductCategoryCommand(
    Guid UserId, 
    Guid ProductId,
    Guid CategoryId, 
    bool IsPrimary) : ICommand;

internal sealed class AddProductCategoryCommandHandler(
    IProductDomainService productDomainService,
    IUnitOfWork unitOfWork,
    ILogger<AddProductCategoryCommandHandler> logger) : ICommandHandler<AddProductCategoryCommand>
{
    public async Task<Result> HandleAsync(AddProductCategoryCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.CategoryId == Guid.Empty)
            return ProductCategoriesErrors.InvalidId;

        try
        {
            var addingResult = await productDomainService.AddProductCategory(
                userId: command.UserId,
                productId: command.ProductId,
                categoryId: command.CategoryId,
                isPrimary: command.IsPrimary,
                ct: ct);

            if (addingResult.IsFailure)
                return addingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Failed To add category with id: '{categoryId}' to product with id: '{productId}'",
                command.CategoryId, command.ProductId);

            return ProductCategoriesErrors.AddProductCategory;
        }
    }
}