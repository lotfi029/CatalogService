using CatalogService.Application.DTOs.Attributes;

namespace CatalogService.Application.Features.Attributes.Queries.GetByCode;

public sealed record GetAttributeByCodeQuery(string Code) : IQuery<AttributeDetailedResponse>;

public sealed class GetAttributeByCodeQueryHandler(
    IAttributeQueries attributeQueries,
    ILogger<GetAttributeByCodeQueryHandler> logger) : IQueryHandler<GetAttributeByCodeQuery, AttributeDetailedResponse>
{
    public async Task<Result<AttributeDetailedResponse>> HandleAsync(GetAttributeByCodeQuery query, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query.Code))
            return AttributeErrors.InvalidId;

        try
        {
            return await attributeQueries.GetByCodeAsync(query.Code, ct);
        }
        catch(Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve attribute with code: {attributeCode}",
                query.Code);
            return AttributeErrors.GetAttributeByCode;
        }
    }
}