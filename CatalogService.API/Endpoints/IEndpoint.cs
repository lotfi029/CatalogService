namespace CatalogService.API.Endpoints;
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
public sealed class TestAuth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth", () =>
        {
            return TypedResults.Ok("you auths");
        }).RequireAuthorization();
    }
}