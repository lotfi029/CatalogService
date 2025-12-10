using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductAttributes.Commands.AddBulk;

public sealed record AddProductAttributeBulkCommand(Guid ProductId, IEnumerable<ProductAttributeBulk> Attribute) : ICommand;
internal sealed class AddProductAttributeBulkCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<AddProductAttributeBulkCommandHandler> logger) : ICommandHandler<AddProductAttributeBulkCommand>
{
    public async Task<Result> HandleAsync(AddProductAttributeBulkCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty)
            return ProductAttributeErrors.InvalidId;

        try
        {
            var addingResult = await productService.AddAttributeBulkAsync(
                productId: command.ProductId,
                command.Attribute.Select(a => (a.AttributeId, a.Value)),
                ct: ct);

            if (addingResult.IsFailure)
                return addingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while adding attributes to product: '{prodictId}'",
                command.ProductId);

            return ProductAttributeErrors.AddProductAttributeBulk;
        }
    }
}