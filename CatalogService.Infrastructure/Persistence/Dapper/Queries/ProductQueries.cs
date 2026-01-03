using CatalogService.Application.DTOs.ProductAttributes;
using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.DTOs.Products;
using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Features.Products.Queries;
using CatalogService.Domain.JsonProperties;

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
                AND c.is_deleted = false
                AND pc.is_active = true;
        
            -- Variants
            SELECT 
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
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

    public async Task<List<ProductDetailedResponse>> GetByIdsAsync(
        List<Guid>? ids,
        CancellationToken ct = default)
    {
        if (ids?.Count == 0)
            return [];

        var connection = connectionFactory.CreateConnection();

        var sql = """
            -- Products
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
            WHERE p.is_deleted = false
                AND (
                    @ids::uuid[] IS NULL
                    OR cardinality(@ids) = 0
                    OR p.id = ANY(@ids)
                );
        
            -- Categories
            SELECT 
                pc.product_id as ProductId,
                c.id as CategoryId,
                c.name as CategoryName,
                c.slug as CategorySlug,
                pc.is_primary as IsPrimary
            FROM public.product_categories pc
            INNER JOIN public.categories c
                ON pc.category_id = c.id
            WHERE (
                    @ids::uuid[] IS NULL
                    OR cardinality(@ids) = 0
                    OR pc.product_id = ANY(@ids)
                )
                AND c.is_active = true
                AND pc.is_active = true
                AND c.is_deleted = false;
        
            -- Variants
            SELECT 
                pv.product_id as ProductId,
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
                pv.price as Price,
                pv.price_currency as Currency,
                pv.compare_at_price as CompareAtPrice
            FROM public.product_variants pv
            WHERE (
                    @ids::uuid[] IS NULL
                    OR cardinality(@ids) = 0
                    OR pv.product_id = ANY(@ids)
                );
        
            -- Attributes
            SELECT 
                pa.product_id as ProductId,
                a.id as AttributeId,
                a.name as AttributeName,
                a.code as AttributeCode,
                a.is_filterable as IsFilterable,
                a.is_searchable as IsSearchable,
                pa.value as AttributeValue
            FROM public.product_attributes pa
            INNER JOIN public.attributes a
                ON pa.attribute_id = a.id
            WHERE (
                    @ids::uuid[] IS NULL
                    OR cardinality(@ids) = 0
                    OR pa.product_id = ANY(@ids)
                )
                AND a.is_active = true
                AND a.is_deleted = false;
            """;

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(sql, new { ids }, cancellationToken: ct));

        var products = (await multi.ReadAsync<ProductDetailedResponse>()).ToList();

        if (!products.Any())
            return [];

        var categories = (await multi.ReadAsync<ProductCategoryWithProductId>())
            .GroupBy(c => c.ProductId)
            .ToDictionary(g => g.Key, g => g.Select(c => new ProductCategoryResponse(
                c.CategoryId,
                c.CategoryName,
                c.CategorySlug,
                c.IsPrimary)).ToList());

        var variants = (await multi.ReadAsync<ProductVariantWithProductId>())
            .GroupBy(v => v.ProductId)
            .ToDictionary(g => g.Key, g => g.Select(v => new ProductVariantResponse(
                v.ProductVariantId,
                v.Sku,
                v.VariantAttributes,
                v.Price,
                v.Currency,
                v.CompareAtPrice)).ToList());

        var attributes = (await multi.ReadAsync<ProductAttributeWithProductId>())
            .GroupBy(a => a.ProductId)
            .ToDictionary(g => g.Key, g => g.Select(a => new ProductAttributeResponse(
                a.AttributeId,
                a.AttributeName,
                a.AttributeCode,
                a.IsFilterable,
                a.IsSearchable,
                a.AttributeValue)).ToList());

        var result = products.Select(p => p with
        {
            ProductCategories = categories.TryGetValue(p.Id, out var cats) && cats.Any() ? cats : null,
            ProductVariants = variants.TryGetValue(p.Id, out var vars) && vars.Any() ? vars : null,
            ProductAttributes = attributes.TryGetValue(p.Id, out var attrs) && attrs.Any() ? attrs : null
        }).ToList();

        return result;
    }
    private record ProductCategoryWithProductId(
        Guid ProductId,
        Guid CategoryId,
        string CategoryName,
        string CategorySlug,
        bool IsPrimary);

    private record ProductVariantWithProductId(
        Guid ProductId,
        Guid ProductVariantId,
        string Sku,
        ProductVariantsOption VariantAttributes,
        ProductVariantsOption? CustomizationOptions,
        decimal Price,
        string Currency,
        decimal? CompareAtPrice);

    private record ProductAttributeWithProductId(
        Guid ProductId,
        Guid AttributeId,
        string AttributeName,
        string AttributeCode,
        bool IsFilterable,
        bool IsSearchable,
        string AttributeValue);


}