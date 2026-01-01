using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.Features.ProductCategories.Command.AddCategory;
using CatalogService.Application.Features.ProductCategories.Command.Delete;
using CatalogService.Application.Features.ProductCategories.Command.DeleteAll;
using CatalogService.Application.Features.ProductCategories.Command.Patch;
using CatalogService.Application.Features.ProductCategories.Queries.Get;
using CatalogService.Application.Features.ProductCategories.Queries.GetByProductId;

namespace CatalogService.API.Endpoints;

internal sealed class ProductCategoriesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/{productId:guid}/categories")
            .MapToApiVersion(1);

        group.MapPost("/{categoryId:guid}", AddCategory)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapPatch("/{categoryId:guid}", UpdateCategory)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapDelete("/{categoryId:guid}", DeleteCategory)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapPatch("/", DeleteAll)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/{categoryId:guid}", Get)
            .Produces<ProductCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapGet("", GetByProductId)
            .Produces<IEnumerable<ProductCategoryResponse>>(StatusCodes.Status200OK);
    }
    private async Task<IResult> AddCategory(
        [FromRoute] Guid productId,
        [FromRoute] Guid categoryId,
        [FromBody] ProductCategoryRequest request,
        [FromServices] IValidator<ProductCategoryRequest> validator,
        [FromServices] ICommandHandler<AddProductCategoryCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddProductCategoryCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId,
            CategoryId: categoryId,
            request.IsPrimary!.Value,
            Request: request.CategoryVariants);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UpdateCategory(
        [FromRoute] Guid productId,
        [FromRoute] Guid categoryId,
        [FromQuery] bool isPrimary,
        [FromServices] ICommandHandler<PatchProductCategoryCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new PatchProductCategoryCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId,
            CategoryId: categoryId,
            isPrimary);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteCategory(
        [FromRoute] Guid productId,
        [FromRoute] Guid categoryId,
        [FromServices] ICommandHandler<DeleteProductCategoryCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteProductCategoryCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId,
            CategoryId: categoryId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAll(
        [FromRoute] Guid productId,
        [FromServices] ICommandHandler<DeleteAllProductCategoryCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteAllProductCategoryCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid productId,
        [FromRoute] Guid categoryId,
        [FromServices] IQueryHandler<GetProductCategoryByIdQuery, ProductCategoryResponse> handler,
        CancellationToken ct)
    {
        var query = new GetProductCategoryByIdQuery(
            ProductId: productId,
            CategoryId: categoryId);

        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByProductId(
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<GetProductCategoriesByProductIdQuery, IEnumerable<ProductCategoryResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetProductCategoriesByProductIdQuery(
            ProductId: productId);

        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
}
