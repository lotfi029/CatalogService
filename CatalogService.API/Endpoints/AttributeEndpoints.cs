using CatalogService.API.EndpointNames;
using CatalogService.Application.DTOs.Attributes;
using CatalogService.Application.Features.Attributes.Command.Activate;
using CatalogService.Application.Features.Attributes.Command.Create;
using CatalogService.Application.Features.Attributes.Command.Deactivate;
using CatalogService.Application.Features.Attributes.Command.Delete;
using CatalogService.Application.Features.Attributes.Command.UpdateDetails;
using CatalogService.Application.Features.Attributes.Command.UpdateOptions;
using CatalogService.Application.Features.Attributes.Queries.Get;
using CatalogService.Application.Features.Attributes.Queries.GetAll;
using CatalogService.Application.Features.Attributes.Queries.GetByCode;
using CatalogService.Application.Features.Attributes.Queries.GetByType;

namespace CatalogService.API.Endpoints;

internal sealed class AttributeEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/attributes")
            .WithTags(AttributeEndpointsNames.Tage)
            .MapToApiVersion(1);

        group.MapPost("/", Create)
            .Produces<Guid>(statusCode: StatusCodes.Status201Created)
            .ProducesProblem(statusCode: StatusCodes.Status409Conflict)
            .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
            .WithDisplayName("Create New Attribute");
        
        group.MapPost("/bulk", CreateBulk);
        
        group.MapPut("/{id:guid}/details", UpdateDetails)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}/options", UpdateOptions)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/{id:guid}/activate", Activate)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/{id:guid}/deactivate", Deactivate)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        
        group.MapGet("", GetAll)
            .Produces<AttributeResponse>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}/", Get)
            .Produces<AttributeDetailedResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName(AttributeEndpointsNames.GetAttributeById);

        group.MapGet("/code/{code:alpha}", GetByCode)
            .Produces<AttributeDetailedResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/type/{type:alpha}", GetByType)
            .Produces<AttributeResponse>(StatusCodes.Status200OK);
    }

    private async Task<IResult> Create(
        [FromBody] CreateAttributeRequest request,
        [FromServices] IValidator<CreateAttributeRequest> validator,
        [FromServices] ICommandHandler<CreateAttributeCommand, Guid> handler,
        CancellationToken ct) 
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateAttributeCommand(
            Name: request.Name,
            Code: request.Code,
            OptionType: request.OptionsType,
            IsFilterable: request.IsFilterable,
            IsSearchable: request.IsSearchable,
            Options: request.Options);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(result.Value, AttributeEndpointsNames.GetAttributeById, new { id = result.Value })
            : result.ToProblem();
    }
    private Task<IResult> CreateBulk() { throw new NotImplementedException(); }
    private async Task<IResult> UpdateDetails(
        [FromRoute]Guid id,
        [FromBody] UpdateAttributeDetailsRequest request,
        [FromServices] IValidator<UpdateAttributeDetailsRequest> validator,
        [FromServices] ICommandHandler<UpdateAttributeDetailsCommand> handler,
        CancellationToken ct) 
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateAttributeDetailsCommand(
            id,
            request.Name,
            IsFilterable: request.IsFilterable,
            IsSearchable: request.IsSearchable);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UpdateOptions(
        [FromRoute] Guid id,
        [FromBody] UpdateAttributeOptionRequest request,
        [FromServices] IValidator<UpdateAttributeOptionRequest> validator,
        [FromServices] ICommandHandler<UpdateAttributeOptionsCommand> handler,
        CancellationToken ct) 
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return TypedResults.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateAttributeOptionsCommand(
            id, request.Option);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Delete(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeleteAttributeCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteAttributeCommand(id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> Activate(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ActivateAttributeCommand> handler,
        CancellationToken ct) 
    {
        var command = new ActivateAttributeCommand(id);
        
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();

    }
    private async Task<IResult> Deactivate(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeactivateAttributeCommand> handler,
        CancellationToken ct)
    {
        var command = new DeactivateAttributeCommand(id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.ToProblem();
    }
    
    private async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllAttributeQuery, IEnumerable<AttributeResponse>> handler,
        CancellationToken ct) 
    { 
        var query = new GetAllAttributeQuery();
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> Get(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetAttributeByIdQuery, AttributeDetailedResponse> handler,
        CancellationToken ct) 
    { 
        var query = new GetAttributeByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByCode(
        [FromRoute] string code,
        [FromServices] IQueryHandler<GetAttributeByCodeQuery, AttributeDetailedResponse> handler,
        CancellationToken ct)
    {
        var query = new GetAttributeByCodeQuery(code);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByType(
        [FromRoute] string type,
        [FromServices] IQueryHandler<GetAttributeByTypeQuery, IEnumerable<AttributeResponse>> handler,
        CancellationToken ct) 
    {
        var query = new GetAttributeByTypeQuery(type);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblem();
    }
}
