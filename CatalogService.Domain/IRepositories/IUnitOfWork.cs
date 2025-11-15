using CatalogService.Domain.Entities;

namespace CatalogService.Domain.IRepositories;
public interface IUnitOfWork { }

public interface IRepository<T> where T : Entity;
public interface IProductRepsitory : IRepository<Product> { }
public interface ICategoryRepository : IRepository<Category> { }
public interface IVariantDefinitionRepository : IRepository<VariantAttributeDefinition> { }
public interface IAttributeDefinitionRepository : IRepository<Entities.Attribute> { }





