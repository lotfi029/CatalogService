using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductCategories.Command.DeleteAll;
public sealed record DeleteAllProductCategoryCommand(
    Guid UserId,
    Guid ProductId) : ICommand;

internal sealed class DeleteAllProductCategoryCommandHandler(
    IUnitOfWork unitOfWork,
    IProductDomainService productDomainService,
    ILogger<DeleteAllProductCategoryCommandHandler> logger) : ICommandHandler<DeleteAllProductCategoryCommand>
{
    public async Task<Result> HandleAsync(DeleteAllProductCategoryCommand command, CancellationToken ct = default)
    {
        if (command.UserId == Guid.Empty || command.ProductId == Guid.Empty)
            return ProductErrors.InvalidId;

        try
        {
            var result = await productDomainService.DeleteAllCategoryAsync(
                userId: command.UserId,
                productId: command.ProductId,
                ct: ct);

            if (result.IsFailure)
                return result.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();

        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to handler delete all product category command. UserId: '{userId}', ProductId: '{productId}'",
                command.UserId, command.ProductId);

            return ProductCategoriesErrors.DeleteProductCategory;
        }
    }
}