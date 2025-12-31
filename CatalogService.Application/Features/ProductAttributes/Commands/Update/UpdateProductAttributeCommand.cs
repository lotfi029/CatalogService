using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductAttributes.Commands.Update;

public sealed record UpdateProductAttributeCommand(Guid UserId, Guid ProductId, Guid AttributeId, string Value) : ICommand;
internal sealed class UpdateProductAttributeCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductAttributeCommandHandler> logger) : ICommandHandler<UpdateProductAttributeCommand>
{
    public async Task<Result> HandleAsync(UpdateProductAttributeCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.AttributeId == Guid.Empty)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var updateResult = await productService.UpdateAttributeValueAsync(
                userId: command.UserId,
                productId: command.ProductId,
                attributeId: command.AttributeId,
                newValue: command.Value,
                ct: ct);

            if (updateResult.IsFailure)
                return updateResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while update attribute: '{attributeId}' in product: '{prodictId}'",
                command.AttributeId, command.ProductId);

            return ProductAttributeErrors.AddProductAttribute;
        }
    }
}
