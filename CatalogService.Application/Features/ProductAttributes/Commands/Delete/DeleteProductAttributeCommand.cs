using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductAttributes.Commands.Delete;

public sealed record DeleteProductAttributeCommand(Guid ProductId, Guid AttributeId) : ICommand;
internal sealed class DeleteProductAttributeCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteProductAttributeCommandHandler> logger) : ICommandHandler<DeleteProductAttributeCommand>
{
    public async Task<Result> HandleAsync(DeleteProductAttributeCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.AttributeId == Guid.Empty)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var deleteResult = await productService.DeleteAttributeAsync(
                productId: command.ProductId,
                attributeId: command.AttributeId,
                ct: ct);

            if (deleteResult.IsFailure)
                return deleteResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while delete attribute: '{attributeId}' from product: '{prodictId}'",
                command.AttributeId, command.ProductId);

            return ProductAttributeErrors.AddProductAttribute;
        }
    }
}