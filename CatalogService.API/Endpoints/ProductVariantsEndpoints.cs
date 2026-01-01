using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Features.ProductVariants.Commands.Delete;
using CatalogService.Application.Features.ProductVariants.Commands.DeleteAll;
using CatalogService.Application.Features.ProductVariants.Commands.UpdateCustomOptions;
using CatalogService.Application.Features.ProductVariants.Commands.UpdatePrice;
using CatalogService.Application.Features.ProductVariants.Queries.Get;
using CatalogService.Application.Features.ProductVariants.Queries.GetByProductId;
using CatalogService.Application.Features.ProductVariants.Queries.GetBySku;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace CatalogService.API.Endpoints;

internal sealed class ProductVariantsEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/product-variants")
            .MapToApiVersion(1);

        group.MapPut("/{productVariantId:guid}/custom-options", UpdateCustomOptions)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{productVariantId:guid}/price", UpdatePrice)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        
        group.MapDelete("/{productVariantId:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
        
        group.MapDelete("/product/{productId:guid}", DeleteAll)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{productVariantId:guid}", Get)
            .Produces<ProductVariantResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/by-product/{productId:guid}",GetByProductId)
            .Produces<ProductVariantResponse[]>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapGet("sku", GetBySku)
            .Produces<ProductVariantResponse[]>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

    }

    private async Task<IResult> UpdateCustomOptions(
        [FromRoute] Guid productVariantId,
        [FromBody] UpdateProductVariantCustomOptionsRequest request,
        [FromServices] IValidator<UpdateProductVariantCustomOptionsRequest> validator,
        [FromServices] ICommandHandler<UpdateProductVariantCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationError)
            return TypedResults.ValidationProblem(validationError.ToDictionary());

        var userId = httpContext.GetUserId();
        var command = new UpdateProductVariantCommand(
            UserId: Guid.Parse(userId),
            productVariantId, 
            request.CustomVariant);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UpdatePrice(
        [FromRoute] Guid productVariantId,
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
            productVariantId, 
            Price: request.Price,
            CompareAtPrice: request.CompareAtPrice,
            Currency: request.Currency);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Delete(
        [FromRoute] Guid productVariantId,
        [FromQuery] Guid productId,
        [FromServices] ICommandHandler<DeleteProductVariantCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteProductVariantCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId, 
            ProductVariantId: productVariantId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAll(
        [FromRoute] Guid productId,
        [FromServices] ICommandHandler<DeleteAllProductVariantCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var command = new DeleteAllProductVariantCommand(
            UserId: Guid.Parse(userId),
            ProductId: productId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid productVariantId,
        [FromServices] IQueryHandler<GetProductVariantByIdQuery, ProductVariantResponse> handler,
        CancellationToken ct)
    {
        var query = new GetProductVariantByIdQuery(productVariantId);
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