using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class VariantDefinitionRepository(ApplicationDbContext context)
    : Repository<VariantAttributeDefinition>(context),
      IVariantDefinitionRepository
{
    
}