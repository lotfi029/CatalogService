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
            	p.is_active as IsActive,
            	pc.is_primary as IsPrimary,
            	c.name as Category,
            	c.slug as CategorySlug,
            	pv.variant_attributes as VariantAttributes,
            	pv.customization_options as CustomizationOptions,
            	pv.price as Price,
            	pv.compare_at_price as CompareAtPrice,
            	a.name as AttributeName,
            	a.is_filterable as IsFilterable,
            	a.is_searchable as IsSearchable,
            	pa.value as AttributeValue
            from public.products p
            Left Join public.product_categories pc
            	on p.id = pc.product_id
            left join public.categories c
            	on pc.category_id = c.id
            left join public.product_variants pv
            	on pv.product_id = p.id
            left join public.product_attributes pa
            	on pa.product_id = p.id
            left join public.attributes a
            	on a.id = pa.attribute_id
            where p.id = '019af01e-7670-78a2-a9dc-b4e15686fc45'
            	AND p.is_active = true
            	AND c.is_active = true
            	AND a.is_active = true
            	AND p.is_deleted = false
            	AND c.is_deleted = false
            	AND a.is_deleted = false
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