using CatalogService.Application.DTOs.CategoryVariantAttributes;
using CatalogService.Application.Features.CategoryVariants.Queries;
namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

public sealed class CategoryVariantAttributeQueries(
    IDbConnectionFactory connectionFactory) : ICategoryVariantAttributeQueries
{
    public async Task<Result<CategoryVariantAttributeDetailedResponse>> Getsync(Guid categoryId, Guid variantId, CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                cva.category_id as CategoryId,
                cva.variant_attribute_id as VariantAttributeId,
                va.name as Name,
                va.code as Code,
                va.data_type_name as Datatype,
                va.allowed_values as AllowedValues,
                va.affects_inventory as AffectedInventory,
                cva.display_order as DisplayOrder,
                cva.is_required as IsRequired,
                cva.created_at as CreatedAt
            FROM public.category_variant_attributes cva
            LEFT JOIN public.variant_attribute_definitions va
            on cva.variant_attribute_id = va.id
            where cva.variant_attribute_id = @variantId
            	AND cva.category_id = @categoryId
            """;

        var parameters = new { categoryId, variantId };

        var command = new CommandDefinition(commandText: sql, parameters: parameters, cancellationToken: ct);
        var response = await connection.QuerySingleOrDefaultAsync<CategoryVariantAttributeDetailedResponse>(command: command);

        if (response is null)
            return CategoryVariantAttributeErrors.NotFound(categoryId, variantId);


        return Result.Success(response);
            
    }

    public async Task<Result<IEnumerable<CategoryVariantAttributeDetailedResponse>>> GetByCategoryIdAsync(
        Guid categoryId, CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                cva.category_id as CategoryId,
                cva.variant_attribute_id as VariantAttributeId,
                cva.is_required as IsRequired,
                cva.display_order as DisplayOrder,
                cva.created_at as CreatedAt,
                va.code as Code,
                va.name as Name,
                va.data_type_name as Datatype,
                va.allowed_values as AllowedValues,
                va.affects_inventory as AffectsInventory
            FROM public.category_variant_attributes cva
            INNER JOIN public.variant_attribute_definitions va 
                ON cva.variant_attribute_id = va.id
            WHERE cva.category_id = @categoryId
                AND va.is_deleted = false
                AND va.is_active = true
            ORDER BY cva.display_order
            """;

        var result = await connection.QueryAsync<CategoryVariantAttributeDetailedResponse>(
            new CommandDefinition(sql, new { categoryId }, cancellationToken: ct));

        return Result.Success(result);
    }
}