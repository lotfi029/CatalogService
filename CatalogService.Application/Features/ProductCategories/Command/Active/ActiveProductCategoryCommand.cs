using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.Active;

public sealed record ActiveProductCategoryCommand(
    Guid UserId,
    Guid ProductId,
    Guid CategoryId) : ICommand;

internal sealed class ActiveProductCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IProductDomainService productDomainService,
    ILogger<ActiveProductCategoryCommandHandler> logger) : ICommandHandler<ActiveProductCategoryCommand>
{
    public async Task<Result> HandleAsync(ActiveProductCategoryCommand command, CancellationToken ct = default)
    {
        try
        {
            var activationResult = await productDomainService.ActiveCategoryAsync(
                userId: command.UserId,
                productId: command.ProductId,
                categoryId: command.CategoryId,
                ct: ct);

            if (activationResult.IsFailure)
                return activationResult.Error;
            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to handle active product command. ProductId: '{productId}', CategoryId: '{categoryId}'",
                command.ProductId, command.CategoryId);
            return ProductCategoriesErrors.ActiveProductCategory;
        }
    }
}