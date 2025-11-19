using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.Specifications;

public abstract class Specification<TEntity>
    where TEntity : Entity
{
    protected Specification()
    {
        
    }
}
