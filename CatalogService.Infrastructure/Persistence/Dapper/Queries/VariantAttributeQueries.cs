using CatalogService.Application.DTOs.VariantAttributes;
using CatalogService.Application.Features.VariantAttributes.Queries;
using CatalogService.Domain.Errors;
using Dapper;
namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

public sealed class VariantAttributeQueries(
    IDbConnectionFactory connectionFactory) : IVariantAttributeQueries
{
    public async Task<Result<VariantAttributeResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                v.id as Id,
                v.code as Code,
                v.name as Name,
                v.data_type_name as Datatype,
                v.allowed_values as AllowedValues,
                v.affects_inventory as AffectsInventory,
                v.affects_pricing as AffectsPricing,
                v.display_order as DisplayOrder
            FROM public.variant_attribute_definitions v
            WHERE v.id = @id
                AND v.is_deleted = false
            """;

        var result = await connection.QuerySingleOrDefaultAsync(
            new CommandDefinition(sql, new { id }, cancellationToken: ct));

        if (result is null)
            return VariantAttributeErrors.NotFound(id);

        return Result.Success(result);
    }

    public async Task<Result<IEnumerable<VariantAttributeResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        using var connection = connectionFactory.CreateConnection();

        const string sql = """
            SELECT
                v.id as Id,
                v.code as Code,
                v.name as Name,
                v.data_type_name as Datatype,
                v.allowed_values as AllowedValues,
                v.affects_inventory as AffectsInventory,
                v.affects_pricing as AffectsPricing,
                v.display_order as DisplayOrder
            FROM public.variant_attribute_definitions v
            WHERE v.is_deleted = false
            """;

        var result = await connection.QueryAsync<VariantAttributeResponse>(
            new CommandDefinition(sql, cancellationToken: ct));

        if (result is null)
            return Result.Success(Enumerable.Empty<VariantAttributeResponse>());

        return Result.Success(result);
    }
}