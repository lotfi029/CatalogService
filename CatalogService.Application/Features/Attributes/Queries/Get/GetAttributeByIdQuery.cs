using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.Get;

public sealed record GetAttributeByIdQuery(Guid Id) : IQuery<AttributeDetailedResponse>;

public sealed class GetAttributeByIdQueryHandler(
    IAttributeQueries attributeQueries,
    ILogger<GetAttributeByIdQueryHandler> logger) : IQueryHandler<GetAttributeByIdQuery, AttributeDetailedResponse>
{
    public async Task<Result<AttributeDetailedResponse>> HandleAsync(GetAttributeByIdQuery query, CancellationToken ct = default)
    {
        if (query.Id == Guid.Empty)
            return AttributeErrors.InvalidId;

        try
        {
            return await attributeQueries.GetAsync(query.Id, ct);
        }
        catch (Exception ex) 
        {
            logger.LogError(ex,
                "Error ocurred while retrieve attribute with id: {attributeId}",
                query.Id);
            return AttributeErrors.GetAttributeById;
        }
    }
}