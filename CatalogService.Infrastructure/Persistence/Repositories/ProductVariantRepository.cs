using CatalogService.Domain.IRepositories;


namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductVariantRepository(ApplicationDbContext context)
    : Repository<ProductVariant>(context), IProductVariantRepository
{
}