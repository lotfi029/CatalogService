namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCategoryRemovedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
