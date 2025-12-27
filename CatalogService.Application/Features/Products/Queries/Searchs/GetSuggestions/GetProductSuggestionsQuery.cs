using CatalogService.Application.Interfaces;

namespace CatalogService.Application.Features.Products.Queries.Searchs.GetSuggestions;

public sealed record GetProductSuggestionsQuery(string Prefix, int Size) : IQuery<List<string>>;

internal sealed class GetProductSuggestionsQueryHandler(
    IProductSearchService productSearchService,
    ILogger<GetProductSuggestionsQueryHandler> logger) : IQueryHandler<GetProductSuggestionsQuery, List<string>>
{
    public async Task<Result<List<string>>> HandleAsync(GetProductSuggestionsQuery query, CancellationToken ct = default)
    {
        try
        {
            return await productSearchService
                .GetSuggestionsAsync(query.Prefix, query.Size, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,"Failed to retrieve product suggestions. prefex: '{prefex}'", query.Prefix);
            return ProductErrors.GetSuggestions;
        }
    }
}