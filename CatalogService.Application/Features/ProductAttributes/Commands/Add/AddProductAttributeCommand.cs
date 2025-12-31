using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductAttributes.Commands.Add;

public sealed record AddProductAttributeCommand(Guid UserId, Guid ProductId, Guid AttributeId, string Value) : ICommand;
internal sealed class AddProductAttributeCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<AddProductAttributeCommandHandler> logger) : ICommandHandler<AddProductAttributeCommand>
{
    public async Task<Result> HandleAsync(AddProductAttributeCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty || command.AttributeId == Guid.Empty)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var addingResult = await productService.AddAttributeAsync(
                userId: command.UserId,
                productId: command.ProductId,
                attributeId: command.AttributeId,
                value: command.Value,
                ct: ct);

            if (addingResult.IsFailure)
                return addingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while adding attribute: '{attributeId}' to product: '{prodictId}'",
                command.AttributeId, command.ProductId);

            return ProductAttributeErrors.AddProductAttribute;
        }
    }
}