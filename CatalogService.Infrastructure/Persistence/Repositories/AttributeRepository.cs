using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class AttributeRepository(ApplicationDbContext context)
    : Repository<Domain.Entities.Attribute>(context), IAttributeRepository
{
    
}