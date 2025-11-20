using CatalogService.Domain.IRepositories;

namespace CatalogService.Infrastructure.Persistence.Repositories;

public sealed class AttributeRepository(
    ApplicationDbContext context,
    ILogger<Repository<Domain.Entities.Attribute>> repositoryLogger)
    : Repository<Domain.Entities.Attribute>(context, repositoryLogger), IAttributeRepository
{
    
}