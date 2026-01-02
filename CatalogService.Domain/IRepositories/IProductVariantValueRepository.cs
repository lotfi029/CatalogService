using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CatalogService.Domain.IRepositories;

public interface IProductVariantValueRepository
{
    void Add(ProductVariantValue value);
    void AddRange(ProductVariantValue[]  values);
    void Update(ProductVariantValue value);
    void UpdateRange(ProductVariantValue[]  values);
    void Delete(ProductVariantValue value);
    void DeleteRange(ProductVariantValue[]  values);
    Task<bool> ExistsAsync(Guid Id, Guid productVariantId, CancellationToken ct = default);
    Task<bool> ExistsAsync(
        Expression<Func<ProductVariantValue, bool>> predicate, 
        CancellationToken ct = default);
    Task<int> ExecuteDeleteAsync(
        Expression<Func<ProductVariantValue, bool>> predicate, 
        CancellationToken ct = default);
    Task<int> ExecuteUpdateAsync(
        Expression<Func<ProductVariantValue, bool>> predicate, 
        Action<UpdateSettersBuilder<ProductVariantValue>> action, 
        CancellationToken ct = default);

    Task<IEnumerable<ProductVariantValue>> GetAllAsync(Guid productVariantId, CancellationToken ct = default);
    Task<HashSet<Guid>> GetProductVariantIdsAsync(
        Expression<Func<ProductVariantValue, bool>> predicate,
        CancellationToken ct);
    Task<ProductVariantValue?> GetById(Guid id, CancellationToken ct = default);
}