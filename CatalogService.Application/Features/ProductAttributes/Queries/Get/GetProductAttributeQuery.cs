using CatalogService.Application.DTOs.ProductAttributes;

namespace CatalogService.Application.Features.ProductAttributes.Queries.Get;

public sealed record GetProductAttributeQuery(Guid ProductId, Guid AttributeId) : IQuery<ProductAttributeResponse>;

internal sealed class GetProductVariantQueryHandler(
    IProductAttributeQueries productAttributeQueries,
    ILogger<GetProductVariantQueryHandler> logger) : IQueryHandler<GetProductAttributeQuery, ProductAttributeResponse>
{
    public async Task<Result<ProductAttributeResponse>> HandleAsync(GetProductAttributeQuery query, CancellationToken ct = default)
    {
        if (Guid.Empty == query.ProductId || Guid.Empty == query.AttributeId)
            return ProductAttributeErrors.InvalidId;
        
        try
        {
            return await productAttributeQueries.GetAsync(query.ProductId, query.AttributeId, ct);
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error while retrieve attribute: '{attributeId}' owned by product: '{productid}'",
                query.AttributeId, query.ProductId);
            return ProductAttributeErrors.GetProductAttribute;
        }
    }
}