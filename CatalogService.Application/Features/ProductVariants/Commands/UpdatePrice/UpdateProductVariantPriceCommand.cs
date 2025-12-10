using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.ProductVariants.Commands.UpdatePrice;

public sealed record UpdateProductVariantPriceCommand(Guid Id, decimal Price, decimal? CompareAtPrice, string Currency) : ICommand;

internal sealed class UpdateProductVariantPriceCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductVariantPriceCommand> logger) : ICommandHandler<UpdateProductVariantPriceCommand>
{
    public async Task<Result> HandleAsync(UpdateProductVariantPriceCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return ProductVariantErrors.InvalidId;
        try
        {
            var updatingResult = await productService.UpdateProductVariantPriceAsync(
                command.Id,
                price: command.Price,
                compareAtPrice: command.CompareAtPrice,
                currency: command.Currency,
                ct);

            if (updatingResult.IsFailure)
                return updatingResult.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while update product variant with id: {productVariantId}",
                command.Id);
            return ProductVariantErrors.UpdateProductVariant;
        }
    }
}