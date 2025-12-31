using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductVariants.Commands.DeleteAll;

public sealed record DeleteAllProductVariantCommand(Guid UserId, Guid ProductId) : ICommand;

internal sealed class DeleteAllProductVariantCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteAllProductVariantCommandHandler> logger) : ICommandHandler<DeleteAllProductVariantCommand>
{
    public async Task<Result> HandleAsync(DeleteAllProductVariantCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty)
            return ProductVariantErrors.InvalidId;

        try
        {
            if (await productService.DeleteAllProductVariantAsync(command.UserId, command.ProductId, ct) is { IsFailure: true } deletingError)
                return deletingError.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while deleting all product variant with id: {id}",
                command.ProductId);
            return ProductVariantErrors.DeleteAllProductVariant;
        }
    }
}