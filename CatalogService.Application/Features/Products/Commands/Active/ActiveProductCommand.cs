using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.Products.Commands.Active;

public sealed record ActiveProductCommand(Guid Id) : ICommand;

internal sealed class ActiveProductCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<ActiveProductCommandHandler> logger) : ICommandHandler<ActiveProductCommand>
{
    public async Task<Result> HandleAsync(ActiveProductCommand command, CancellationToken ct = default)
    {
        if (command.Id == Guid.Empty)
            return ProductErrors.InvalidId;
        try
        {
            var result = await productService.ActivaAsync(command.Id, ct: ct);
            if (result.IsFailure)
                return result.Error;

            await unitOfWork.SaveChangesAsync(ct: ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while activate the product with id: {productId}",
                command.Id);
            return ProductErrors.ActiveProduct;
        }
    }
}