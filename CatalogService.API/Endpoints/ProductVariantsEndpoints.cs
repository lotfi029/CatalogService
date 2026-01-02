using CatalogService.API.EndpointNames;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Features.ProductVariants.Commands.Add;
using CatalogService.Application.Features.ProductVariants.Commands.Delete;
using CatalogService.Application.Features.ProductVariants.Commands.UpdatePrice;
using CatalogService.Application.Features.ProductVariants.Queries.Get;
using CatalogService.Application.Features.ProductVariants.Queries.GetByProductId;
using CatalogService.Application.Features.ProductVariants.Queries.GetBySku;

namespace CatalogService.API.Endpoints;

internal sealed class ProductVariantsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/product-variants")
            .WithTags(ProductVariantEndpointNames.Tags)
            .MapToApiVersion(1);

        group.MapPut("/", Add)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Vendor);

        group.MapPut("/{variantId:guid}/price", Update)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Vendor);
        
        group.MapDelete("/{variantId:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.Vendor);

        group.MapGet("/{variantId:guid}", Get)
            .Produces<ProductVariantResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/by-product/{productId:guid}",GetByProductId)
            .Produces<ProductVariantResponse[]>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapGet("/sku", GetBySku)
            .Produces<ProductVariantResponse[]>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

    }
    private async Task<IResult> Add(
        [FromBody] ProductVariantRequest request,
        [FromServices] IValidator<ProductVariantRequest> validator,
        [FromServices] ICommandHandler<AddProductVariantCommand> handler,
        HttpContext httpContext,
        CancellationToken ct = default
        )
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationErrors)
            return TypedResults.ValidationProblem(validationErrors.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new AddProductVariantCommand(
            Guid.Parse(userId),
            request.ProductId,
            request.Price,
            request.CompareAtPrice,
            request.Variants.ToDictionary(x => x.VariantId, x => x.Value));

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Update(
        [FromRoute] Guid variantId,
        [FromBody] UpdateProductVariantPriceRequest request,
        [FromServices] IValidator<UpdateProductVariantPriceRequest> validator,
        [FromServices] ICommandHandler<UpdateProductVariantPriceCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationError)
            return TypedResults.ValidationProblem(validationError.ToDictionary());
        var userId = httpContext.GetUserId();
        var command = new UpdateProductVariantPriceCommand(
            UserId: Guid.Parse(userId),
            variantId, 
            Price: request.Price,
            CompareAtPrice: request.CompareAtPrice,
            Currency: request.Currency);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Delete(
        [FromRoute] Guid variantId,
        [FromQuery] Guid productId,
        [FromServices] ICommandHandler<DeleteProductVariantCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteProductVariantCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId, 
            ProductVariantId: variantId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid variantId,
        [FromServices] IQueryHandler<GetProductVariantByIdQuery, ProductVariantResponse> handler,
        CancellationToken ct)
    {
        var query = new GetProductVariantByIdQuery(variantId);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByProductId(
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<GetProductVariantByProductIdQuery, List<ProductVariantResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetProductVariantByProductIdQuery(productId);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetBySku(
        [FromQuery] string sku,
        [FromServices] IQueryHandler<GetProductVariantBySkuQuery, List<ProductVariantResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetProductVariantBySkuQuery(sku);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
}