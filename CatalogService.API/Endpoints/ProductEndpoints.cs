using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Commands.Active;
using CatalogService.Application.Features.Products.Commands.Archive;
using CatalogService.Application.Features.Products.Commands.Create;
using CatalogService.Application.Features.Products.Commands.CreateBulk;
using CatalogService.Application.Features.Products.Commands.UpdateDetails;
using CatalogService.Application.Features.Products.Queries.Get;
using CatalogService.Application.Features.Products.Queries.GetAll;

namespace CatalogService.API.Endpoints;

internal sealed class ProductEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .MapToApiVersion(1);

        group.MapPost("/", Create)
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        group.MapPost("/bulk", CreateBulk)
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", UpdateDetails)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/{id:guid}/active", Active)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPatch("/{id:guid}/archive", Archive)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/{id:guid}", Get)
            .Produces<ProductDetailedResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        group.MapGet("/", GetAll)
            .Produces<IEnumerable<ProductResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        

    }
    private async Task<IResult> Create(
        [FromBody] ProductRequest request,
        [FromServices] IValidator<ProductRequest> validator,
        [FromServices] ICommandHandler<CreateProductCommand, Guid> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateProductCommand(Guid.NewGuid(), request.Name, request.Description);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.Created()
            : result.ToProblem();
    }
    private async Task<IResult> CreateBulk(
        [FromBody] CreateBulkProductsRequest request,
        [FromServices] IValidator<CreateBulkProductsRequest> validator,
        [FromServices] ICommandHandler<CreateBulkProductCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateBulkProductCommand(Guid.NewGuid(), request);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.Created()
            : result.ToProblem();
    }
    private async Task<IResult> UpdateDetails(
        [FromRoute] Guid id,
        [FromBody] ProductRequest request,
        [FromServices] IValidator<ProductRequest> validator,
        [FromServices] ICommandHandler<UpdateProductDetailsCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateProductDetailsCommand(id, request.Name, request.Description);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Active(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ActiveProductCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ActiveProductCommand(id);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Archive(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ArchiveProductCommand> handler,
        CancellationToken ct = default)
    {
        var command = new ArchiveProductCommand(id);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid id,
        [FromServices]IQueryHandler<GetProductByIdQuery, ProductDetailedResponse> handler,
        CancellationToken ct)
    {
        var query = new GetProductByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }

    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllProductsQuery();
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    
}
