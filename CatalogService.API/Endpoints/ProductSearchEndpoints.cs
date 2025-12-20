using CatalogService.Application.Interfaces;
using CatalogService.Infrastructure.Search.Elasticsearch.IndexManager;

namespace CatalogService.API.Endpoints;

public class ProductSearchEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/search")
            .WithTags("Product Search");

        group.MapGet("/", Search);
    }
    private async Task<IResult> Search(
        [FromServices] IProductSearchService productSearch,
        [FromServices] IElasticsearchIndexManager indexManager,
        CancellationToken ct)
    {
        var indexResult = await indexManager.CreateProductIndexAsync(ct);

        if (indexResult is false)
            return TypedResults.BadRequest("Failed to index the product");

        var response = await productSearch.SearchProductsAsync(
            searchTerm: "produc",
            categoryIds: [],
            filters: [],
            ct: ct);

        return TypedResults.NoContent();
    }
}
