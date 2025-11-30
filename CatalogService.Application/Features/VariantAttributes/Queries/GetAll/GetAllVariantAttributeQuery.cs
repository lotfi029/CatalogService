using CatalogService.Application.DTOs.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Queries.GetAll;

public sealed record GetAllVariantAttributeQuery : IQuery<IEnumerable<VariantAttributeResponse>>;

public sealed class GetAllVariantAttributeQueryHandler(
    ILogger<GetAllVariantAttributeQueryHandler> logger,
    IVariantAttributeQueries attributeQueries) : IQueryHandler<GetAllVariantAttributeQuery, IEnumerable<VariantAttributeResponse>>
{
    public async Task<Result<IEnumerable<VariantAttributeResponse>>> HandleAsync(GetAllVariantAttributeQuery command, CancellationToken ct = default)
    {
        try
        {
            return await attributeQueries.GetAllAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error ocurred while retrieving variant attribute definition");
            return Error.Unexpected("Error ocurred while retrieving variant attribute definition");
        }
    }
}