using CatalogService.Application.DTOs.CategoryVariantAttributes;
using CatalogService.Application.Features.CategoryVariants.Commands.AddBulkVariants;
using CatalogService.Application.Features.CategoryVariants.Commands.AddVariant;
using CatalogService.Application.Features.CategoryVariants.Commands.RemoveVariant;
using CatalogService.Application.Features.CategoryVariants.Commands.UpdateVariant;
using CatalogService.Application.Features.CategoryVariants.Queries.Get;
using CatalogService.Application.Features.CategoryVariants.Queries.GetAll;

namespace CatalogService.API.Endpoints;

internal sealed class CategoryVariantEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("categories/{categoryId:guid}/variant-attributes")
            .MapToApiVersion(1);

        group.MapPost("/", Add)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Admin);
        
        group.MapPost("/bulk",  AddBulk)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Admin);
        
        group.MapPut("/{variantAttributeId:guid}", Update)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Admin);
        
        group.MapDelete("/{variantAttributeId:guid}", Remove)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Admin);

        group.MapGet("/{variantAttributeId:guid}", Get)
            .Produces<CategoryVariantAttributeDetailedResponse>(statusCode: StatusCodes.Status200OK)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);

        group.MapGet("/", GetAll)
            .Produces<IEnumerable<CategoryVariantAttributeDetailedResponse>>(statusCode: StatusCodes.Status200OK)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest);
    }

    private async Task<IResult> Add(
        [FromRoute] Guid categoryId,
        [FromBody] AddCategoryVariantRequest request,
        [FromServices] IValidator<AddCategoryVariantRequest> validator,
        [FromServices] ICommandHandler<AddCategoryVariantCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new AddCategoryVariantCommand(
            Id: categoryId,
            VariantId: request.VariantId,
            DisplayOrder: request.DisplayOrder,
            IsRequired: request.IsRequired);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> AddBulk(
        [FromRoute] Guid categoryId,
        [FromBody] AddCategoryVariantBulkRequest request,
        [FromServices] IValidator<AddCategoryVariantBulkRequest> validator,
        [FromServices] ICommandHandler<AddCategoryVariantBulkCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new AddCategoryVariantBulkCommand(Id: categoryId, Request: request);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Update(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid variantAttributeId,
        [FromBody] UpdateCategoryVariantRequest request,
        [FromServices] IValidator<UpdateCategoryVariantRequest> validator,
        [FromServices] ICommandHandler<UpdateCategoryVariantCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateCategoryVariantCommand(
            Id: categoryId,
            variantAttributeId,
            DisplayOrder: request.DisplayOrder,
            IsRequired: request.IsRequired);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Remove(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid variantAttributeId,
        [FromServices] ICommandHandler<RemoveCategoryVariantCommand> handler,
        CancellationToken ct)
    {
        var command = new RemoveCategoryVariantCommand(
            Id: categoryId,
            VariantId: variantAttributeId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }

    private async Task<IResult> Get(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid variantAttributeId,
        [FromServices] IQueryHandler<GetCategoryVariantAttributeCommand, CategoryVariantAttributeDetailedResponse> handler,
        CancellationToken ct)
    {
        var command = new GetCategoryVariantAttributeCommand(categoryId, variantAttributeId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    
    private async Task<IResult> GetAll(
        [FromRoute] Guid categoryId,
        [FromServices] IQueryHandler<GetAllCategoryVariantAttributesQuery, IEnumerable<CategoryVariantAttributeDetailedResponse>> handler,
        CancellationToken ct)
    {
        var command = new GetAllCategoryVariantAttributesQuery(categoryId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
}
