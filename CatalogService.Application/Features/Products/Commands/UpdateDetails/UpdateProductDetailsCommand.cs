using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.Products.Commands.UpdateDetails;

public sealed record UpdateProductDetailsCommand(
    Guid Id,
    string Name,
    string? Description) : ICommand;

internal sealed class UpdateProductDetailsCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<UpdateProductDetailsCommandHandler> logger) : ICommandHandler<UpdateProductDetailsCommand>
{
    public async Task<Result> HandleAsync(UpdateProductDetailsCommand command, CancellationToken ct = default)
    {
        if (Guid.Empty == command.Id)
            return ProductErrors.InvalidId;

        try
        {
            var productResult = await productService.UpdateDetails(
                id: command.Id,
                name: command.Name,
                description: command.Description,
                ct: ct);

            if (productResult.IsFailure)
                return productResult;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while adding new product");

            return ProductErrors.UpdateProductDetails;
        }
    }
}