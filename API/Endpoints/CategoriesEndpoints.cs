using Application.Features.Categories.Commands;
using Application.Features.Categories.Contracts;
using Application.Features.Categories.Queries;

namespace API.Endpoints;

public class CategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/categories")
            .WithTags("Categories")
            .RequireAuthorization();

        group.MapPost("", Add);

        group.MapPut("{id:guid}", Update);

        group.MapPut("{id:guid}/toggle-is-disable", ToggleIsDisable);

        group.MapGet("{id:guid}", GetById)
            .WithName("GetCategoryById");

        group.MapGet("", GetAll);
    }

    private static async Task<IResult> Add(
        [FromBody] CategoryRequest request,
        IValidator<CategoryRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "Validation failed",
                Detail = "One or more validation errors occurred",
                Instance = "/api/category",
                Status = StatusCodes.Status400BadRequest
            };

            return TypedResults.Problem(problemDetails);
        }

        var command = new AddCategoryCommand(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute("GetCategoryById", new { id = result.Value })
            : result.ToProblemDetails();
    }

    private static async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] CategoryRequest request,
        IValidator<CategoryRequest> _validator,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(validationResult.ToDictionary())
            {
                Title = "Validation failed",
                Detail = "One or more validation errors occurred",
                Instance = "/api/category",
                Status = StatusCodes.Status400BadRequest
            };

            return TypedResults.Problem(problemDetails);
        }

        var command = new UpdateCategoryCommand(id, request);

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
        var command = new ToggleCategoryIsDisableCommand(id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetById(
        [FromRoute] Guid id,
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
    )
    {
        var query = new GetCategoryByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender _sender,
        CancellationToken cancellationToken
    )
    {
        var query = new GetAllCategoriesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return TypedResults.Ok(result);
    }
}
