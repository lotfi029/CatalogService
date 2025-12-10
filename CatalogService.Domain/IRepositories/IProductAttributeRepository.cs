using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;

public interface IProductAttributeRepository
{
    void Add(ProductAttributes productAttributes);
    void AddRange(ProductAttributes[] products);
    void Update(ProductAttributes productAttributes);
    void UpdateRange(ProductAttributes[] products);
    void Delete(ProductAttributes productAttributes);
    void DeleteRange(ProductAttributes[] products);

    Task<IEnumerable<ProductAttributes>> GetAllByProductIdAsync(Guid productId, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<ProductAttributes, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid productId, Guid attributeId, CancellationToken ct = default);
    Task<ProductAttributes?> GetById(Guid productId, Guid attributeId, CancellationToken ct = default);
    Task<int> ExecuteDeleteAsync(Expression<Func<ProductAttributes, bool>> predicate, CancellationToken ct = default);
}
