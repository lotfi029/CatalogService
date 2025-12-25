namespace CatalogService.Domain.DomainEvents.Products.ProductCategories;

public sealed record ProductCategoryAddedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
