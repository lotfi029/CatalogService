using CatalogService.Application.DTOs.ProductVariants;
using CatalogService.Application.Features.ProductVariants.Queries;

namespace CatalogService.Infrastructure.Persistence.Dapper.Queries;

internal sealed class ProductVariantQueries(
    IDbConnectionFactory dbConnectionFactory) : IProductVariantQueries
{
    public async Task<Result<ProductVariantResponse>> GetAsync(Guid productVariantId, CancellationToken ct = default)
    {
        var connection = dbConnectionFactory.CreateConnection();
        var sql = """
            SELECT 
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
                pv.customization_options as CustomizationOptions,
                pv.price as Price,
                pv.price_currency as Currency,
                pv.compare_at_price as CompareAtPrice
            FROM public.product_variants pv
            Where pv.id = @id
            """;

        var productVariant = await connection.QuerySingleOrDefaultAsync<ProductVariantResponse>(
            new CommandDefinition(commandText: sql, parameters: new { id = productVariantId }, cancellationToken: ct));

        if (productVariant is null)
            return ProductVariantErrors.NotFound(productVariantId);

        return productVariant;
    }
    public async Task<Result<List<ProductVariantResponse>>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        var connection = dbConnectionFactory.CreateConnection();
        var sql = """
            SELECT 
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
                pv.customization_options as CustomizationOptions,
                pv.price as Price,
                pv.price_currency as Currency,
                pv.compare_at_price as CompareAtPrice
            FROM public.product_variants pv
            Where pv.product_id = @productId
            """;

        var productVariants = await connection.QueryAsync<ProductVariantResponse>(
            new CommandDefinition(commandText: sql, parameters: new { productId }, cancellationToken: ct));

        if (productVariants is null)
            return Result.Success<List<ProductVariantResponse>>([]);

        return productVariants.ToList();
    }
    public async Task<Result<List<ProductVariantResponse>>> GetSkuAsync(string sku, CancellationToken ct = default)
    {
        var connection = dbConnectionFactory.CreateConnection();
        var sql = """
            SELECT 
                pv.id as ProductVariantId,
                pv.sku as Sku,
                pv.variant_attributes as VariantAttributes,
                pv.customization_options as CustomizationOptions,
                pv.price as Price,
                pv.price_currency as Currency,
                pv.compare_at_price as CompareAtPrice
            FROM public.product_variants pv
            Where pv.sku = @sku
            """;

        var productVariant = await connection.QueryAsync<ProductVariantResponse>(
            new CommandDefinition(commandText: sql, parameters: new { sku }, cancellationToken: ct));

        if (productVariant is null || !productVariant.Any())
            return Result.Success<List<ProductVariantResponse>>([]);

        return productVariant.ToList();
    }
}