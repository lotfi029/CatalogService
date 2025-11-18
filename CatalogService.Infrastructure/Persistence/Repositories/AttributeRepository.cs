using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class AttributeRepository(ApplicationDbContext context)
    : Repository<Domain.Entities.Attribute>(context), IAttributeRepository
{
    
}