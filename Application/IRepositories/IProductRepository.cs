namespace Application.IRepositories;
public interface IProductRepository
{
    Task<Result<Guid>> AddProductAsync(Product request, CancellationToken cancellationToken = default);
    Task<Result> UpdateProductAsync(Product request, CancellationToken cancellationToken = default);
    Task<Result<Product>> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllProductAsync(bool? includeDisabled = null, CancellationToken cancellationToken = default);
}