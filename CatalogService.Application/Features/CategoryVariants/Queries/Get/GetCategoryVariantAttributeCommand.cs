using CatalogService.Application.DTOs.CategoryVariantAttributes;

namespace CatalogService.Application.Features.CategoryVariants.Queries.Get;

public sealed record GetCategoryVariantAttributeCommand(Guid CategoryId, Guid VariantAttributeId) : IQuery<CategoryVariantAttributeDetailedResponse>;

public sealed class GetCategoryVariantAttributeCommandHandler(
    ILogger<GetCategoryVariantAttributeCommandHandler> logger,
    ICategoryVariantAttributeQueries variantQueries) : IQueryHandler<GetCategoryVariantAttributeCommand, CategoryVariantAttributeDetailedResponse>
{
    public async Task<Result<CategoryVariantAttributeDetailedResponse>> HandleAsync(GetCategoryVariantAttributeCommand command, CancellationToken ct = default)
    {
        try
        {
            return await variantQueries.Getsync(command.CategoryId, command.VariantAttributeId, ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error ocurred while retrieve category variant attribute with category id: {categoryId} and variant attribute id: {variantId}",
                command.CategoryId, command.VariantAttributeId);

            return CategoryVariantAttributeErrors.GetCategoryVariantAttribute;
        }
    }
}