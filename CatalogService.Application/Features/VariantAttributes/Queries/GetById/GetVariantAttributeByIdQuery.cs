using CatalogService.Application.DTOs.VariantAttributes;

namespace CatalogService.Application.Features.VariantAttributes.Queries.GetById;

public sealed record GetVariantAttributeByIdQuery(Guid Id) : IQuery<VariantAttributeResponse>;

public sealed class GetVariantAttributeByIdQueryHandler(
    ILogger<GetVariantAttributeByIdQueryHandler> logger,
    IVariantAttributeQueries variantQueries) : IQueryHandler<GetVariantAttributeByIdQuery, VariantAttributeResponse>
{
    public async Task<Result<VariantAttributeResponse>> HandleAsync(GetVariantAttributeByIdQuery query, CancellationToken ct = default)
    {
        try
        {
            return await variantQueries.GetByIdAsync(query.Id, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieving variant attribute definition with id {id}", query.Id);
            return Error.Unexpected($"Error ocurred while retrieving variant attribute definition with id {query.Id}");
        }
    }
}