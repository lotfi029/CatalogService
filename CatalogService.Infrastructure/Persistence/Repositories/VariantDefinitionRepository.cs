using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class VariantDefinitionRepository(
    ApplicationDbContext context,
    ILogger<Repository<VariantAttributeDefinition>> logger)
    : Repository<VariantAttributeDefinition>(context, logger),
      IVariantDefinitionRepository
{
    
}