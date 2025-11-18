using CatalogService.Domain.IRepositories;
using CatalogService.Infrastructure.Persistence.Contexts;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class VariantDefinitionRepository(ApplicationDbContext context)
    : Repository<VariantAttributeDefinition>(context),
      IVariantDefinitionRepository
{
    
}