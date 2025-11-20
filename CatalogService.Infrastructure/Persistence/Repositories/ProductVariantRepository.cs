using CatalogService.Domain.IRepositories;


namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductVariantRepository(
    ApplicationDbContext context,
    ILogger<Repository<ProductVariant>> logger)
    : Repository<ProductVariant>(context, logger), IProductVariantRepository
{
}