namespace CatalogService.Application.Interfaces;

public interface IBulkReindexService
{
    Task ReindexAllProductsAsync(CancellationToken ct = default);
    Task ReindexAllCategoriesAsync(CancellationToken ct = default);
    Task ReindexAllAttributesAsync(CancellationToken ct = default);
    Task ReindexAllAsync(CancellationToken ct = default);
}
