using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductVariants.Commands.Delete;

public sealed record DeleteProductVariantCommand(Guid ProductId, Guid ProductVariantId) : ICommand;

internal sealed class DeleteProductVariantCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteProductVariantCommandHandler> logger) : ICommandHandler<DeleteProductVariantCommand>
{
    public async Task<Result> HandleAsync(DeleteProductVariantCommand command, CancellationToken ct = default)
    {
        if (command.ProductVariantId == Guid.Empty || command.ProductId == Guid.Empty)
            return ProductVariantErrors.InvalidId;

        try
        {
            if (await productService.DeleteProductVariantAsync(command.ProductId, command.ProductVariantId, ct) is { IsFailure: true } deletingError)
                return deletingError.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while deleting product variant with id: {id}",
                command.ProductVariantId);
            return ProductVariantErrors.DeleteProductVariant;
        }
    }
}