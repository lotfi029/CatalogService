using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Commands;

namespace CatalogService.API.Endpoints;

internal sealed class ICategoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1/api/categories");

        group.MapPost("/", Create)
            .Produces<Guid>(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    private async Task<IResult> Create(
        [FromBody] CreateCategoryRequest request,
        [FromServices] IValidator<CreateCategoryRequest> validator,
        [FromServices] ICommandHandler<CreateCategoryCommand, Guid> handler,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateCategoryCommand(
            Name: request.Name,
            Slug: request.Slug,
            IsActive: request.IsActive,
            ParentId: request.ParentId,
            Description: request.Description);

        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? TypedResults.Created()
            : result.ToProblem();
    }
}