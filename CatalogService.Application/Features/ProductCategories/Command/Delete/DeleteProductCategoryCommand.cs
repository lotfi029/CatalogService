using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.Delete;

public sealed record DeleteProductCategoryCommand(
    Guid UserId,
    Guid ProductId,
    Guid CategoryId) : ICommand;

internal sealed class DeleteProductCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IProductDomainService productDomainService,
    ILogger<DeleteProductCategoryCommandHandler> logger) : ICommandHandler<DeleteProductCategoryCommand>
{
    public async Task<Result> HandleAsync(DeleteProductCategoryCommand command, CancellationToken ct = default)
    {
        if (command.UserId == Guid.Empty || command.ProductId == Guid.Empty || command.CategoryId == Guid.Empty)
            return ProductErrors.InvalidId;

        try
        {
            var result = await productDomainService.DeleteCategoryAsync(
                userId: command.UserId,
                productId: command.ProductId,
                categoryId: command.CategoryId,
                ct: ct);

            if (result.IsFailure)
                return result.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();

        } catch(Exception ex)
        {
            logger.LogError(ex,
                "Failed to handler delete product category command. UserId: '{userId}', ProductId: '{productId}', CategoryId: '{categoryId}'",
                command.UserId, command.ProductId, command.CategoryId);

            return ProductCategoriesErrors.DeleteProductCategory;
        }
    }
}