using CatalogService.API.Extensions;
using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Commands.Create;
using CatalogService.Application.Features.Categories.Commands.Delete;
using CatalogService.Application.Features.Categories.Commands.Move;
using CatalogService.Application.Features.Categories.Commands.UpdateDetails;

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
        
        group.MapPost("/{id:guid}/update-details", UpdateDetails)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .MapToApiVersion(1);

        group.MapPut("/{id:guid}/move", Move)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);

        group.MapDelete("/{id:guid}", Delete)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}", GetById)
            .Produces(statusCode: StatusCodes.Status200OK)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .MapToApiVersion(1);

        group.MapGet("/slug/{slug:alpha}", GetBySlug)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .MapToApiVersion(1);

        group.MapGet("/{id:guid}/products", GetProducts)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);

        group.MapGet("/tree", GetTree)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
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
        return result.Match(TypedResults.Created, CustomResults.ToProblem);
    }
    private async Task<IResult> UpdateDetails(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryDetailsRequest request,
        [FromServices] IValidator<UpdateCategoryDetailsRequest> validator,
        [FromServices] ICommandHandler<UpdateCategoryDetailsCommand> handler,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        var command = new UpdateCategoryDetailsCommand(id, request);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(TypedResults.NoContent, CustomResults.ToProblem);
    }
    private async Task<IResult> Move(
        [FromRoute] Guid id,
        [FromQuery] Guid newParent,
        [FromServices] ICommandHandler<MoveCategoryToNewParentCommand> handler,
        CancellationToken ct

        )
    {
        var command = new MoveCategoryToNewParentCommand(id, newParent);
        var result = await handler.HandleAsync(command, ct);
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private async Task<IResult> Delete(
        [FromRoute] Guid id,
        [FromQuery] Guid? moveProductTo,
        [FromServices] ICommandHandler<DeleteCategoryCommand> handler,
        CancellationToken ct
        )
    {
        throw new NotImplementedException();
    }
    private async Task GetTree(
        [FromQuery] Guid? parentId,
        [FromQuery] int? maxDepth)
    {
        throw new NotImplementedException();
    }

    private async Task GetById(
        [FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
    private async Task GetBySlug(
        [FromRoute] string slug)
    {
        throw new NotImplementedException();
    }
    private async Task GetProducts(
        [FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}