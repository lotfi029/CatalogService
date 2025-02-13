using Application.Features.Products.Command;
using Application.Features.Products.Contract;
using Application.Features.Products.Queries;
using Application.Services;

namespace API.Endpoints;

public class ProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/product")
            .WithTags("Products")
            .DisableAntiforgery()
            .RequireAuthorization();

        group.MapPost("", Add);

        group.MapPut("{id:guid}", Update);
        
        group.MapPut("{id:guid}/toggle-is-disable", ToggleIsDisable);
        
        group.MapPut("{id:guid}/delete", DeleteProduct);

        group.MapGet("{id:guid}", Get)
            .WithName("GetProductById");

        group.MapGet("", GetAll);

        group.MapGet("category/{categoryId:guid}", ProductInCategory);

        group.MapGet("image/{imageName}", StreamImage)
            .WithName("stream-image")
            .AllowAnonymous();
    }
    private static async Task<IResult> Add(
        [FromForm] ProductRequest request,
        IValidator<ProductRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "validate failed",
                Detail = "one or more validation errors occured",
                Instance = "/api/register",
                Status = StatusCodes.Status400BadRequest
            };

            return TypedResults.Problem(problemDetails);
        }

        var command = new AddProductCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute("GetProductById", new { id = result.Value })
            : result.ToProblemDetails();
    }
    private static async Task<IResult> Update(
        [FromRoute] Guid id, 
        [FromBody] UpdateProductRequest request,
        IValidator<UpdateProductRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "validate failed",
                Detail = "one or more validation errors occured",
                Instance = "/api/register",
                Status = StatusCodes.Status400BadRequest
            };

            return TypedResults.Problem(problemDetails);
        }

        var command = new UpdateProductCommand(id, request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private static async Task<IResult> ToggleIsDisable(
        [FromRoute] Guid id,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        
        var command = new ToggleProductIsDisableCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private static async Task<IResult> DeleteProduct(
        [FromRoute] Guid id, 
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var command = new DeleteProductCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }
    private static async Task<IResult> Get(
        [FromRoute] Guid id,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var command = new GetProductByIdQuery(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }
    private static async Task<IResult> GetAll(
        [FromServices] ISender _sender,
        LinkGenerator _linkGenerator,
        HttpContext _httpContext,
        CancellationToken cancellationToken
        )
    {
        var command = new GetAllProductsQuery();
        
        var result = await _sender.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }
    private static async Task<IResult> ProductInCategory(
        [FromRoute] Guid categoryId,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
        )
    {
        var command = new GetProductInCategoryQuery(categoryId);

        var result = await _sender.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }
    private static async Task<IResult> StreamImage(
        string imageName,
        [FromServices] IFileService _fileService
        )
    {
        var (stream, contentType) = await _fileService.GetImageStreamAsync(imageName);
        
        if (stream is null)
            return TypedResults.NotFound();

        return TypedResults.Stream(stream, contentType);
    }

}
