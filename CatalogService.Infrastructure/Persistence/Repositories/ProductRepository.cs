using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(ApplicationDbContext context) 
    : Repository<Product>(context), IProductRepository
{
}

