using CatalogService.API.EndpointNames;
using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.Features.ProductAttributes.Commands.Add;
using CatalogService.Application.Features.ProductAttributes.Commands.AddBulk;
using CatalogService.Application.Features.ProductAttributes.Commands.Delete;
using CatalogService.Application.Features.ProductAttributes.Commands.DeleteAll;
using CatalogService.Application.Features.ProductAttributes.Commands.Update;
using CatalogService.Application.Features.ProductAttributes.Queries.Get;
using CatalogService.Application.Features.ProductAttributes.Queries.GetAll;

namespace CatalogService.API.Endpoints;
internal sealed class ProductAttributesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/{productId:guid}/attributes")
            .WithTags(ProductAttributeEndpointsNames.Tag)
            .MapToApiVersion(1);

        group.MapPost("/{attributeId:guid}", Add)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
        
        group.MapPost("/bulk", AddBulk)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapPut("/{attributeId:guid}", Update)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{attributeId:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{attributeId:guid}/all", DeleteAll)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/", GetByProductId)
            .Produces<List<ProductAttributeResponse>>(StatusCodes.Status200OK);
        
        group.MapGet("/{attributeId:guid}", Get)
            .Produces<ProductAttributeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private async Task<IResult> Add(
        [FromRoute] Guid productId,
        [FromRoute] Guid attributeId,
        [FromBody] ProductAttributeRequest request,
        [FromServices] IValidator<ProductAttributeRequest> validator,
        [FromServices] ICommandHandler<AddProductAttributeCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());
        
        var userId = httpContext.GetUserId();
        var command = new AddProductAttributeCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId, 
            AttributeId: attributeId, 
            request.Value);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> AddBulk(
        [FromRoute] Guid productId,
        [FromBody] ProductAttributeBulkRequest request,
        [FromServices] IValidator<ProductAttributeBulkRequest> validator,
        [FromServices] ICommandHandler<AddProductAttributeBulkCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddProductAttributeBulkCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId, 
            Attribute: request.Attributes);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Update(
        [FromRoute] Guid productId,
        [FromRoute] Guid attributeId,
        [FromBody] ProductAttributeRequest request,
        [FromServices] IValidator<ProductAttributeRequest> validator,
        [FromServices] ICommandHandler<UpdateProductAttributeCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new UpdateProductAttributeCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId, 
            AttributeId: attributeId, 
            request.Value);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Delete(
        [FromRoute] Guid productId,
        [FromRoute] Guid attributeId,
        [FromServices] ICommandHandler<DeleteProductAttributeCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteProductAttributeCommand(
            UserId: Guid.Parse(userId), 
            ProductId: productId, 
            AttributeId: attributeId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAll(
        [FromRoute] Guid productId,
        [FromRoute] Guid attributeId,
        [FromServices] ICommandHandler<DeleteAllProductAttributeCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteAllProductAttributeCommand(
            UserId: Guid.Parse(userId), 
            ProductId: productId, 
            AttributeId: attributeId);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid attributeId,
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<GetProductAttributeQuery, ProductAttributeResponse> handler,
        CancellationToken ct)
    {
        var query = new GetProductAttributeQuery(ProductId: productId, AttributeId: attributeId);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByProductId(
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<GetAllProductAttributeQuery, IEnumerable< ProductAttributeResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllProductAttributeQuery(ProductId: productId);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
}