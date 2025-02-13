using Application.Features.Wishlist.Commands;
using Application.Features.Wishlist.Queries;
using System.Security.Claims;

namespace API.Endpoints;

public class WishListEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/wishlist")
            .WithTags("WishList")
            .RequireAuthorization();

        group.MapPost("{productId:guid}", Add);
        group.MapGet("", GetAll);
        group.MapDelete("{id:guid}", Delete);

    }
    private static async Task<IResult> Add(
        [FromRoute] Guid productId,
        [FromServices] ISender _sender,
        HttpContext _httpContext,
        CancellationToken cancellationToken
        )
    {
        var UserId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new AddWishListCommand(UserId! ,productId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created()
            : result.ToProblemDetails();
    }
    
    private static async Task<IResult> Delete(
        [FromRoute] Guid id,
        [FromServices] ISender _sender,
        HttpContext _httpContext,
        CancellationToken cancellationToken
        )
    {
        var userId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new RemoveWishListCommand(userId, id);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok()
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAll(
        [FromServices] ISender _sender,
        HttpContext _httpContext,
        CancellationToken cancellationToken
        )
    {
        var UserId = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var command = new GetWishListProductQuery(UserId);

        var result = await _sender.Send(command, cancellationToken);

        return TypedResults.Ok(result);
    }
}
