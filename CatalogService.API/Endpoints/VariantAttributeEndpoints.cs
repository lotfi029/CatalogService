using Asp.Versioning;
using CatalogService.Application.DTOs.VariantAttributes;
using CatalogService.Application.Features.VariantAttributes.Commands.Create;
using CatalogService.Application.Features.VariantAttributes.Commands.Delete;
using CatalogService.Application.Features.VariantAttributes.Commands.Update;
using CatalogService.Application.Features.VariantAttributes.Queries.GetAll;
using CatalogService.Application.Features.VariantAttributes.Queries.GetById;

namespace CatalogService.API.Endpoints;

internal sealed class VariantAttributeEndpoints : IEndpoint
{
    private readonly ApiVersion _apiVersion = new(1);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("variant-attributes");


        group.MapPost("", Create)
            .Produces<Guid>()
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .MapToApiVersion(_apiVersion);
        
        group.MapPut("/{id:guid}", Update)
            .Produces(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .MapToApiVersion(_apiVersion);

        group.MapDelete("/{id:guid}", Delete)
            .Produces(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .MapToApiVersion(_apiVersion);
        
        group.MapGet("/{id:guid}", GetById)
            .WithName(nameof(GetById))
            .Produces<VariantAttributeResponse>()
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .MapToApiVersion(_apiVersion);
        
        group.MapGet("", GetAll)
            .Produces<VariantAttributeResponse>()
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
            .MapToApiVersion(_apiVersion);
    }

    private async Task<IResult> Create(
        [FromBody] CreateVariantAttributeRequest reqeust,
        [FromServices] IValidator<CreateVariantAttributeRequest> validator,
        [FromServices] ICommandHandler<CreateVariantAttributeCommand, Guid> handler,
        CancellationToken ct
        )
    {
        if (await validator.ValidateAsync(reqeust, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateVariantAttributeCommand(reqeust);
        var result = await handler.HandleAsync(command, ct);  

        
        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                routeName: nameof(GetById),
                routeValues: new {id = result.Value, version = _apiVersion},
                value: result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVariantAttributeRequest request,
        [FromServices] IValidator<UpdateVariantAttributeRequest> validator,
        [FromServices] ICommandHandler<UpdateVariantAttributeCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateVariantAttributeCommand(id, request);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Delete(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeleteVariantAttributeCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteVariantAttributeCommand(id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetVariantAttributeByIdQuery, VariantAttributeResponse> handler,
        CancellationToken ct
        )
    {

        var query = new GetVariantAttributeByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    
    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllVariantAttributeQuery, IEnumerable<VariantAttributeResponse>> handler,
        CancellationToken ct
        )
    {

        var query = new GetAllVariantAttributeQuery();
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }

}