using CatalogService.Application.DTOs.Products;
using CatalogService.Application.Features.Products.Queries;

namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

internal sealed class ProductQueries(
    IDbConnectionFactory connectionFactory) : IProductQueries
{
    public async Task<Result<ProductDetailedResponse>> GetAsync(Guid id, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT
            	p.id as Id,
            	p.name as Name,
            	p.description as Description,
            	p.vendor_id as VendorId,
            	CASE 
            		WHEN p.status = 1 then 'Draft'
            		WHEN p.status = 2 then 'Active'
            		WHEN p.status = 3 then 'Inactive'
            		WHEN p.status = 4 then 'Archive'
            	END as Status,
            	p.created_at as CreatedAt,
            	p.last_updated_at as LastUpdatedAt,
            	p.is_active as IsActive
            FROM public.products p
            WHERE p.id = @id
            """;

        var result = await connection.QuerySingleOrDefaultAsync<ProductDetailedResponse>(
            command: new CommandDefinition(commandText: sql, parameters: new { id }, cancellationToken: ct));

        if (result is null)
            return ProductErrors.NotFound(id);

        return result;
    }
    public async Task<Result<IEnumerable<ProductResponse>>> GetAllAsync(CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            SELECT
            	p.id as Id,
            	p.name as Name,
            	p.description as Description,
            	p.vendor_id as VendorId,
            	CASE 
            		WHEN p.status = 1 then 'Draft'
            		WHEN p.status = 2 then 'Active'
            		WHEN p.status = 3 then 'Inactive'
            		WHEN p.status = 4 then 'Archive'
            	END as Status
            FROM public.products p
            """;

        var result = await connection.QueryAsync<ProductResponse>(new CommandDefinition(commandText: sql, cancellationToken: ct));

        if (result is null)
            return Result.Success(Enumerable.Empty<ProductResponse>());

        return Result.Success(result);
    }
}