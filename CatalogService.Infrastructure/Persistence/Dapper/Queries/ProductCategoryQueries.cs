using CatalogService.Application.DTOs.ProductCategories;
using CatalogService.Application.Features.ProductCategories.Queries;

namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

internal sealed class ProductCategoryQueries(
    IDbConnectionFactory contextFactory) : IProductCategoryQueries
{
    public async Task<Result<ProductCategoryResponse>> GetAsync(Guid productId, Guid categoryId, CancellationToken ct = default)
    {
        using var connection = contextFactory.CreateConnection();

        var sql = """
            SELECT 
                c.id as CategoryId,
                c.name as CategoryName,
                c.slug as CategorySlug,
                pc.is_primary as IsPrimary
            FROM public.product_categories pc
            INNER JOIN public.categories c
                ON pc.category_id = c.id
            WHERE pc.product_id = @productId
                AND pc.categoryId = @categoryId
                AND c.is_active = true
                AND c.is_deleted = false;
            """;

        var parameters = new { productId, categoryId };
        var response = await connection.QuerySingleOrDefaultAsync<ProductCategoryResponse>(
            new CommandDefinition(sql, parameters: parameters, cancellationToken: ct));

        if (response is null)
            return ProductCategoriesErrors.NotFound;
        
        return response;
    }
    public async Task<IEnumerable<ProductCategoryResponse>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        using var connection = contextFactory.CreateConnection(); // for streek

        var sql = """
            SELECT 
                c.id as CategoryId,
                c.name as CategoryName,
                c.slug as CategorySlug,
                pc.is_primary as IsPrimary
            FROM public.product_categories pc
            INNER JOIN public.categories c
                ON pc.category_id = c.id
            WHERE pc.product_id = @productId
                AND c.is_active = true
                AND c.is_deleted = false;
            """;

        var response = await connection.QueryAsync<ProductCategoryResponse>(
            new CommandDefinition(sql, parameters: new { productId }, cancellationToken: ct));

        return response ?? [];
    }

}

