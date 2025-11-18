using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(ApplicationDbContext context) 
    : Repository<Product>(context), IProductRepository
{
}

