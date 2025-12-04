using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetByType;

public sealed record GetAttributeByTypeQuery(string OptionsType) : IQuery<IEnumerable<AttributeResponse>>;

public sealed class GetAttributeByTypeQueryHandler(
    IAttributeQueries attributeQueries,
    ILogger<GetAttributeByTypeQueryHandler> logger) : IQueryHandler<GetAttributeByTypeQuery, IEnumerable<AttributeResponse>>
{
    public async Task<Result<IEnumerable<AttributeResponse>>> HandleAsync(GetAttributeByTypeQuery query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query.OptionsType))
            return AttributeErrors.InvalidOptinsType;
        
        try
        {
            return await attributeQueries.GetByOptionsTypeAsync(query.OptionsType, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieving attributes by type: {optionsType}",
                query.OptionsType);

            return AttributeErrors.GetAttributeByType;
        }
    }
}