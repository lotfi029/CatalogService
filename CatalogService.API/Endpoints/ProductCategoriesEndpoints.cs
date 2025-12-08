using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.Features.ProductCategories.Command.AddCategory;

namespace CatalogService.API.Endpoints;

public class ProductCategoriesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/{productId:guid}/categories")
            .MapToApiVersion(1);

        group.MapPost("/{categoryId:guid}", AddCategory)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
    public async Task<IResult> AddCategory(
        [FromRoute] Guid productId,
        [FromRoute] Guid categoryId,
        [FromBody] ProductCategoryRequest request,
        [FromServices] IValidator<ProductCategoryRequest> validator,
        [FromServices] ICommandHandler<AddProductCategoryCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var command = new AddProductCategoryCommand(
            ProductId: productId,
            CategoryId: categoryId,
            request.IsPrimary!.Value,
            Request: request.CategoryVariants);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
}
