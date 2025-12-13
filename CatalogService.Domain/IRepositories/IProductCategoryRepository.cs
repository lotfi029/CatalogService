using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;

public interface IProductCategoryRepository
{
    void Add(ProductCategories productCategory);
    void AddRange(IEnumerable<ProductCategories> productCategories);
    void Update(ProductCategories productCategory);
    void Remove(ProductCategories productCategory);
    void RemoveRange(IEnumerable<ProductCategories> productCategories);
    Task<bool> ExistsAsync(Guid productId, Guid categoryId, CancellationToken ct = default);
    Task<IEnumerable<ProductCategories>> GetByCategoryIdAsync(
        Guid categoryId,
        CancellationToken ct = default);
    Task<IEnumerable<ProductCategories>> GetByProductIdAsync(
        Guid productId,
        CancellationToken ct = default);
    Task<ProductCategories?> GetAsync(
        Guid productId,
        Guid categoryId,
        CancellationToken ct = default);
    Task<int> RemoveAllByProductAsync(Guid productId, CancellationToken ct = default);
    Task<int> RemoveAllByCategoryAsync(Guid categoryId, CancellationToken ct = default);
    Task<int> UpdatePrimaryAsync(Guid productId, Guid categoryId, bool isPrimary, CancellationToken ct = default);
    Task<bool> ExistsAsync(Expression<Func<ProductCategories, bool>> predicate, CancellationToken ct = default);
    Task<int> ExecuteUpdateAsync(Expression<Func<ProductCategories, bool>> predicate, Action<UpdateSettersBuilder<ProductCategories>> action, CancellationToken ct = default);
}