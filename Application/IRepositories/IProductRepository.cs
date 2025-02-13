namespace Application.IRepositories;
public interface IProductRepository
{
    Task<Result<Guid>> AddAsync(Product request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Product request, CancellationToken cancellationToken = default);
    Task<Result<Product>> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(bool? includeDisabled = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetInCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
}