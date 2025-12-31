using CatalogService.Domain.DomainService.Products;

namespace CatalogService.Application.Features.Products.Commands.Archive;

public sealed record ArchiveProductCommand(Guid UserId, Guid Id) : ICommand;

internal sealed class ArchiveProductCommandHandler(
    IProductDomainService productService,
    IUnitOfWork unitOfWork,
    ILogger<ArchiveProductCommandHandler> logger) : ICommandHandler<ArchiveProductCommand>
{
    public async Task<Result> HandleAsync(ArchiveProductCommand command, CancellationToken ct)
    {
        if (command.Id == Guid.Empty)
            return ProductErrors.InvalidId;

        try
        {
            if (await productService.ArchiveAsync(command.UserId, command.Id, ct) is { IsFailure: true } archivedError)
                return archivedError.Error;

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, 
                "An error occurred while archiving the product with Id: {ProductId}", 
                command.Id);
            return ProductErrors.ArchiveProduct;
        }
    }
}