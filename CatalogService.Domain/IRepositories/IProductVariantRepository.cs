
namespace CatalogService.Domain.IRepositories;

public interface IProductVariantRepository
{
    void Add(ProductVariant productVariant);
    void AddRange(ProductVariant[] products);
    void Update(ProductVariant productVariant);
    void UpdateRange(ProductVariant[] products);
    void Delete(ProductVariant productVariant);
    void DeleteRange(ProductVariant[] products);

    Task<IEnumerable<ProductVariant>> GetAllAsync(Guid productId, CancellationToken ct = default);
    Task<ProductVariant?> GetById(Guid id, CancellationToken ct = default);
}