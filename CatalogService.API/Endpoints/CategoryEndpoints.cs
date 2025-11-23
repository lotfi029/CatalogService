using Asp.Versioning;
using Asp.Versioning.Builder;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Commands;

namespace CatalogService.API.Endpoints;

internal sealed class CategoryEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("categories");
        
        group.MapPost("/", Create)
            .Produces<Guid>(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .MapToApiVersion(1);

        group.MapGet("/", GetAll)
            .MapToApiVersion(2);
    }

    private async Task<IResult> GetAll()
    {
        return Results.Ok("ok");
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