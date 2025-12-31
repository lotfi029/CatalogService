using CatalogService.Domain.DomainService.Products;
using CatalogService.Domain.JsonProperties;

namespace CatalogService.Application.Features.ProductVariants.Commands.UpdateCustomOptions;

public sealed record UpdateProductVariantCommand(Guid UserId, Guid Id, ProductVariantsOption CustomOptions) : ICommand;

internal sealed class UpdateProductVariantCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductVariantCommandHandler> logger) : ICommandHandler<UpdateProductVariantCommand>
{
    public async Task<Result> HandleAsync(UpdateProductVariantCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return ProductVariantErrors.InvalidId;
        try
        {
            var updatingResult = await productService.UpdateProductVariantCustomizationOptionsAsync(
                command.UserId,
                command.Id,
                command.CustomOptions,
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