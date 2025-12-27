using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Products.Queries.Searchs.Search;

public sealed record SearchProductQuery(
    string? SearchTerm,
    List<Guid>? CategoryIds,
    Dictionary<string, List<string>>? Filters,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page,
    int Size
    ) : IQuery<(IEnumerable<ProductDetailedResponse> products, long Total)>;



internal sealed class SearchProductQueryHandler(
    IProductSearchService productSearchService,
    ILogger<SearchProductQueryHandler> logger) : IQueryHandler<SearchProductQuery, (IEnumerable<ProductDetailedResponse> products, long Total)>
{
    public async Task<Result<(IEnumerable<ProductDetailedResponse> products, long Total)>> HandleAsync(SearchProductQuery query, CancellationToken ct = default)
    {
        try
        {
            return await productSearchService.SearchProductsAsync(
                searchTerm: query.SearchTerm,
                categoryIds: query.CategoryIds,
                filters: query.Filters,
                minPrice: query.MinPrice,
                maxPrice: query.MaxPrice,
                from: (query.Page - 1) * query.Size,
                size: query.Size,
                ct: ct);
        }
        catch ( Exception ex )
        {
            logger.LogError(ex,
                "Failed to search products");

            return ProductErrors.SearchProducts;
        }
    }
}