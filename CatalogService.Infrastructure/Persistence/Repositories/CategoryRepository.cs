using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class CategoryRepository(ApplicationDbContext context)
    : Repository<Category>(context), ICategoryRepository
{
}