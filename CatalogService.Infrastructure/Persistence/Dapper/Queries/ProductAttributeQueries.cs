using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.Features.ProductAttributes.Queries;
namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

internal sealed class ProductAttributeQueries(
    IDbConnectionFactory connectionFactory) : IProductAttributeQueries
{
    public async Task<Result<ProductAttributeResponse>> GetAsync(Guid productId, Guid attributeId, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.Code as Code,
                pa.value as Value,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable
            FROM public.attributes a 
            INNER JOIN public.product_attributes pa
            on a.id = pa.attribute_id
            where pa.attribute_id = @attributeId 
                AND pa.product_id = @productId
                AND a.is_active = true
                AND a.is_deleted = false
            """;

        var parameters = new { productId, attributeId };

        var response = await connection.QuerySingleOrDefaultAsync<ProductAttributeResponse>(
            new CommandDefinition(commandText: sql, parameters: parameters, cancellationToken: ct));

        if (response is null) 
            return ProductAttributeErrors.NotFound(productId, attributeId);

        return response;
    }
    public async Task<IEnumerable<ProductAttributeResponse>> GetAllByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT 
                a.id as Id,
                a.name as Name,
                a.Code as Code,
                pa.value as Value,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable
            FROM public.attributes a 
            INNER JOIN public.product_attributes pa
            on a.id = pa.attribute_id
            where pa.product_id = @productId
                AND a.is_active = true
                AND a.is_deleted = false
            """;

        var parameters = new { productId };

        var response = await connection.QueryAsync<ProductAttributeResponse>(
            new CommandDefinition(commandText: sql, parameters: parameters, cancellationToken: ct));

        if (response is null)
            return [];

        return response;
    }
}
