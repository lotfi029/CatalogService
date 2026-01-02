using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductVariants.Commands.Add;

public sealed record AddProductVariantCommand(
    Guid UserId,
    Guid ProductId,
    decimal Price,
    decimal? CompareAtPrice,
    Dictionary<Guid, string> Variants) : ICommand;

internal sealed class AddProductVariantCommandHandler(
    IUnitOfWork unitOfWork,
    IProductDomainService productDomainService,
    ILogger<AddProductVariantCommandHandler> logger) : ICommandHandler<AddProductVariantCommand>
{
    public async Task<Result> HandleAsync(AddProductVariantCommand command, CancellationToken ct = default)
    {
        if (command.ProductId == Guid.Empty)
            return ProductVariantErrors.InvalidId;
        try
        {
            var addingResult = await productDomainService.AddProductVariantAsync(
                userId: command.UserId,
                productId: command.ProductId,
                price: command.Price,
                compareAtPrice: command.CompareAtPrice,
                variantAttributes: command.Variants,
                ct);

            if (addingResult.IsFailure)
                return addingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to handler add product variant command. ProductId: {productId}",
                command.ProductId);
            return ProductVariantErrors.AddProductVariant;
        }
    }
}