namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCategoryAddedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
