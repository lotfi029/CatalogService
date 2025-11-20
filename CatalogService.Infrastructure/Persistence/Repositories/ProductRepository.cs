using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(
    ApplicationDbContext context,
    ILogger<Repository<Product>> logger) 
    : Repository<Product>(context, logger), IProductRepository
{
}

