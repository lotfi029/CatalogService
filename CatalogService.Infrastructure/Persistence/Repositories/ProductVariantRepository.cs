using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductVariantRepository(ApplicationDbContext context)
    : Repository<ProductVariant>(context), IProductVariantRepository
{
}