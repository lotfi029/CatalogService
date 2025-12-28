using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.Products.Search;
using CatalogService.Application.Features.Products.Queries.Searchs.GetSuggestions;
using CatalogService.Application.Features.Products.Queries.Searchs.Search;

namespace CatalogService.API.Endpoints;

internal sealed class ProductSearchEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/search")
            .WithTags("Product Search")
            .MapToApiVersion(1);

        group.MapPost("/products", Search)
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/products/suggest", GetProductSuggestions)
            .Produces(StatusCodes.Status200OK);
    }
    private async Task<IResult> Search(
        [FromBody] SearchProductRequest request,
        [FromServices] IQueryHandler<SearchProductQuery, (IEnumerable<ProductDetailedResponse> products, long Total)> handler,
        [FromServices] IValidator<SearchProductRequest> validator,
        CancellationToken ct = default)
    {

        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var query = new SearchProductQuery(
            SearchTerm: request.SearchTerm,
            CategoryIds: request.CategoryIds,
            Filters: request.Filters,
            MinPrice: request.MinPrice,
            MaxPrice: request.MaxPrice,
            Page: request.Page,
            Size: request.Size
            );

        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(new
            {
                Data = result.Value.products,
                result.Value.Total,
                request.Page,
                request.Size,
                TotalPages = (int)Math.Ceiling((double)result.Value.Total / request.Size)
            })
            : result.ToProblem();
    }
    private async Task<IResult> GetProductSuggestions(
        [FromServices] IQueryHandler<GetProductSuggestionsQuery, List<string>> handler,
        [FromQuery] string prefix,
        [FromQuery] int size,
        CancellationToken ct = default)
    {
        var query = new GetProductSuggestionsQuery(
            prefix,
            size);

        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }

}
