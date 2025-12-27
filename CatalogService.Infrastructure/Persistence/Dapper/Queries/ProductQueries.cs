using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Features.Products.Queries;

namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

internal sealed class ProductQueries(
    IDbConnectionFactory connectionFactory) : IProductQueries
{
    public async Task<Result<ProductDetailedResponse>> GetAsync(Guid id, CancellationToken ct = default)
    {
        var connection = connectionFactory.CreateConnection();

        var sql = """
            -- Product
            SELECT 
                p.id as Id,
                p.name as Name,
                p.description as Description,
                p.vendor_id as VendorId,
                CASE 
                    WHEN p.status = 1 THEN 'Draft'
                    WHEN p.status = 2 THEN 'Active'
                    WHEN p.status = 3 THEN 'Inactive'
                    WHEN p.status = 4 THEN 'Archive'
                    ELSE 'Unspecified'
                END as Status,
                p.created_at as CreatedAt,
                p.last_updated_at as LastUpdatedAt,
                p.is_active as IsActive
            FROM public.products p
            WHERE p.id = @id
                AND p.is_deleted = false;
        
            -- Categories
            SELECT 
                c.id as CategoryId,
                c.name as CategoryName,
                c.slug as CategorySlug,
                pc.is_primary as IsPrimary
            FROM public.product_categories pc
            INNER JOIN public.categories c
                ON pc.category_id = c.id
            WHERE pc.product_id = @id
                AND c.is_active = true
                AND c.is_deleted = false;
        
            -- Variants
            SELECT 
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
                pv.customization_options as CustomizationOptions,
                pv.price as Price,
                pv.price_currency as Currency,
                pv.compare_at_price as CompareAtPrice
            FROM public.product_variants pv
            WHERE pv.product_id = @id
                AND pv.is_deleted = false;
                
        
            -- Attributes
            SELECT 
                a.id as AttributeId,
                a.name as AttributeName,
                a.code as AttributeCode,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable,
                pa.value as AttributeValue
            FROM public.product_attributes pa
            INNER JOIN public.attributes a
                ON pa.attribute_id = a.id
            WHERE pa.product_id = @id
                AND a.is_active = true
                AND a.is_deleted = false;
            """;
        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { id }, cancellationToken: ct));

        if (await multi.ReadSingleOrDefaultAsync<ProductDetailedResponse>() is not { } product)
            return ProductErrors.NotFound(id);

        var categories = await multi.ReadAsync<ProductCategoryResponse>();
        var variants = await multi.ReadAsync<ProductVariantResponse>();
        var attributes = await multi.ReadAsync<ProductAttributeResponse>();

        var result = product with
        {
            ProductCategories = categories.Any() ? [.. categories] : [],
            ProductVariants = variants.Any() ? [.. variants] : [],
            ProductAttributes = attributes.Any() ? [.. attributes] : []
        };

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