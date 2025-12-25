namespace CatalogService.Domain.DomainEvents.Products.ProductCategories;

public sealed record ProductCategoryUpdatedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
