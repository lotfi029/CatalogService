using CatalogService.Application.DTOs.Categories;
using CatalogService.Application.Features.Categories.Commands.AddVariant;
using CatalogService.Application.Features.Categories.Commands.RemoveVariant;
using CatalogService.Application.Features.Categories.Commands.UpdateVariant;

namespace CatalogService.API.Endpoints;

internal sealed class CategoryVariantEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("categories/{categoryId:guid}/variant-attributes")
            .MapToApiVersion(1);

        group.MapPost("/", AddVariantAttribute)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound);
        
        group.MapPut("/{variantAttributeId:guid}", UpdateVariantAttribute)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound);
        
        group.MapDelete("/{variantAttributeId:guid}", RemoveVariantAttribute)
            .Produces(statusCode: StatusCodes.Status204NoContent)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound);
    }

    private async Task<IResult> AddVariantAttribute(
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
    
    private async Task<IResult> UpdateVariantAttribute(
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
    private async Task<IResult> RemoveVariantAttribute(
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
}
