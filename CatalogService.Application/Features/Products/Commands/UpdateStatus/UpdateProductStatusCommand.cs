namespace CatalogService.Application.Features.Products.Commands.UpdateStatus;

//public sealed record UpdateProductStatusCommand(
//    Guid Id,
//    string Status) : ICommand;

//internal sealed class UpdateProductStatusCommandHandler(
//    IProductDomainService productService,
//    IUnitOfWork unitOfWork,
//    ILogger<UpdateProductStatusCommandHandler> logger) : ICommandHandler<UpdateProductStatusCommand>
//{
//    public async Task<Result> HandleAsync(UpdateProductStatusCommand command, CancellationToken ct = default)
//    {
//        if (Guid.Empty == command.Id)
//            return ProductErrors.InvalidId;

//        try
//        {
//            var productResult = await productService.UpdateProductStatus(
//                id: command.Id,
//                status: command.Status,
//                ct: ct);

//            if (productResult.IsFailure)
//                return productResult;

//            await unitOfWork.SaveChangesAsync(ct);
//            return Result.Success();
//        }
//        catch (Exception ex)
//        {
//            logger.LogError(ex,
//                "Error ocurred while adding new product");

//            return ProductErrors.UpdateProductStatus;
//        }
//    }
//}