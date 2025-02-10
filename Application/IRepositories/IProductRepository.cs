namespace Application.IRepositories;
public interface IProductRepository
{
    Task<Result> AddProductAsync(ProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateProductInfoAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> UpdateProductQuentityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> ToggleProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<Result> GetAllProductAsync(bool includeDisabled = false, CancellationToken cancellationToken = default);
}
