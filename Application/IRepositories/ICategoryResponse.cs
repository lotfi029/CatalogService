namespace Application.IRepositories;
public interface ICategoryResponse
{
    Task<Result<Guid>> AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task<Result<Category>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
}
