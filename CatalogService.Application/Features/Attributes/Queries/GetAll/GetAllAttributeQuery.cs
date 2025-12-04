using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetAll;

public sealed record GetAllAttributeQuery : IQuery<IEnumerable<AttributeResponse>>;

public sealed class GetAllAttributeQueryHandler(
    IAttributeQueries attributeQueries,
    ILogger<GetAllAttributeQueryHandler> logger) : IQueryHandler<GetAllAttributeQuery, IEnumerable<AttributeResponse>>
{
    public async Task<Result<IEnumerable<AttributeResponse>>> HandleAsync(GetAllAttributeQuery query, CancellationToken ct = default)
    {
        try
        {
            return await attributeQueries.GetAllAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve attributes");

            return AttributeErrors.GetAllAttribute;
        }
    }
}