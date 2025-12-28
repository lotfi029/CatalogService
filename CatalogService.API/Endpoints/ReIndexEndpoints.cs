using CatalogService.Application.Features.BulkIndex.Commands.All;
using CatalogService.Application.Features.BulkIndex.Commands.Attributes;
using CatalogService.Application.Features.BulkIndex.Commands.Categories;
using CatalogService.Application.Features.BulkIndex.Commands.Products;

namespace CatalogService.API.Endpoints;

internal sealed class ReIndexEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/re-index/")
            .WithTags("ReIndex")
            .MapToApiVersion(1);


        group.MapPost("/products", ReIndexProducts);
        group.MapPost("/attributes", ReIndexCategories);
        group.MapPost("/categories", ReIndexAttributes);
        group.MapPost("/all", ReIndexAll);


    }

    private async Task<IResult> ReIndexProducts(
        ICommandHandler<ReindexAllProductsCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ReindexAllProductsCommand();
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> ReIndexAttributes(
        ICommandHandler<ReindexAllAttributesCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ReindexAllAttributesCommand();
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> ReIndexCategories(
        ICommandHandler<ReindexAllCategoriesCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ReindexAllCategoriesCommand();
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> ReIndexAll(
        ICommandHandler<ReindexAllCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ReindexAllCommand();
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
}