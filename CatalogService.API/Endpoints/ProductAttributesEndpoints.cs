namespace CatalogService.API.Endpoints;

internal sealed class ProductAttributesEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products/{productId:guid}/attributes")
            .MapToApiVersion(1);

        group.MapPost("/{attributeId:guid}", Add);
        group.MapPut("/{attributeId:guid}", Update);
        group.MapDelete("/{attributeId:guid}", Delete);
        group.MapGet("/", GetByProductId);
        group.MapGet("/{attributeId:guid}", Get);
    }

    private Task<IResult> Add(
        [FromRoute] Guid attributeId)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> Update(
        [FromRoute] Guid attributeId)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> Delete(
        [FromRoute] Guid attributeId)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> Get(
        [FromRoute] Guid attributeId)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> GetByProductId()
    {
        throw new NotImplementedException();
    }

}