namespace CatalogService.Domain.DomainEvents.Products.ProductCategories;

public sealed record ProductCategoryRemovedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
