using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    Guid VendorId,
    string Name,
    string? Description) : ICommand<Guid>;

internal sealed class CreateProductCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateProductCommand command, CancellationToken ct = default)
    {
        try
        {
            var productResult = productService.Create(
                vendorId: command.VendorId,
                name: command.Name,
                description: command.Description);

            if (productResult.IsFailure)
                return productResult;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success(productResult.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while adding new product");

            return ProductErrors.CreateProduct;
        }
    }
}