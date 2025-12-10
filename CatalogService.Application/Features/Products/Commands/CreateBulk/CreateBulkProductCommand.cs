using CatalogService.Application.DTOs.Products;
using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.Products.Commands.CreateBulk;

public sealed record CreateBulkProductCommand(Guid VendorId, CreateBulkProductsRequest Request) : ICommand;

internal sealed class CreateBulkProductCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<CreateBulkProductCommandHandler> logger) : ICommandHandler<CreateBulkProductCommand>
{
    public async Task<Result> HandleAsync(CreateBulkProductCommand command, CancellationToken ct = default)
    {
        if (command.VendorId == Guid.Empty)
            return ProductErrors.InvalidId;

        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var addingResult = productService.CreateBulk(
                vendorId: command.VendorId,
                values: command.Request.Products.Select(p => (p.Name, p.Description)));

            if (addingResult.IsFailure)
            {
                await unitOfWork.RollBackTransactionAsync(transaction, ct);
                return addingResult.Error;
            }
            
            var addedCount = await unitOfWork.SaveChangesAsync(ct);
            
            if (addedCount != command.Request.Products.Count)
            {
                await unitOfWork.RollBackTransactionAsync(transaction, ct);
                return ProductErrors.CreateBulkProduct;
                
            }
            await unitOfWork.CommitTransactionAsync(transaction, ct);
            return Result.Success();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollBackTransactionAsync(transaction, ct);

            logger.LogError(ex,
                "Error ocurred while adding new products");

            return ProductErrors.CreateBulkProduct;
        }
    }
}