using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class VariantAttributeRepository(ApplicationDbContext context)
    : Repository<VariantAttributeDefinition>(context),
      IVariantAttributeRepository
{
    
}