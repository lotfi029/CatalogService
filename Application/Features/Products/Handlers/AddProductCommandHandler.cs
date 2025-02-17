using Application.Errors;
using Application.Features.Products.Command;
using Application.Services;

namespace Application.Features.Products.Handlers;
public class AddProductCommandHandler(
    IProductRepository _repository,
    IUnitOfWork _unitOfWork,
    IFileService _fileService,
    ICategoryRepository _categoryRepository) : IRequestHandler<AddProductCommand, Result<Guid>>
{
    
    public async Task<Result<Guid>> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.GetByIdAsync(command.Request.CategoryId, cancellationToken) is null)
            return Result.Failure<Guid>(CategoryErrors.NotFound);

        var product = new Product
        {
            Name = command.Request.Name,
            Description = command.Request.Description,
            Price = command.Request.Price,
            Quentity = command.Request.Quentity,
            CategoryId = command.Request.CategoryId,
            ImageUrl = await _fileService.UploadImageAsync(command.Request.Image, cancellationToken)
        };

        var result = await _repository.AddAsync(product, cancellationToken);

        if (result.IsFailure)
            return result;


        await _unitOfWork.SaveChangeAsync(cancellationToken);

        // TODO: SignalR
        //await _hubContext.Clients.All.ProductAdded(product.Adapt<ProductResponse>());

        return Result.Success(result.Value);
    }
}
