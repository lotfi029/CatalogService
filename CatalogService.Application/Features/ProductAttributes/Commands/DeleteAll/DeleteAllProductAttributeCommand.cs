using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductAttributes.Commands.DeleteAll;

public sealed record DeleteAllProductAttributeCommand(Guid UserId, Guid ProductId, Guid AttributeId) : ICommand;
internal sealed class DeleteAllProductAttributeCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<DeleteAllProductAttributeCommandHandler> logger) : ICommandHandler<DeleteAllProductAttributeCommand>
{
    public async Task<Result> HandleAsync(DeleteAllProductAttributeCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.AttributeId == Guid.Empty)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var deleteResult = await productService.DeleteAllAttributeAsync(
                userId: command.UserId,
                productId: command.ProductId,
                ct: ct);

            if (deleteResult.IsFailure)
                return deleteResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while delete all attribute: '{attributeId}' from product: '{prodictId}'",
                command.AttributeId, command.ProductId);

            return ProductAttributeErrors.AddProductAttribute;
        }
    }
}