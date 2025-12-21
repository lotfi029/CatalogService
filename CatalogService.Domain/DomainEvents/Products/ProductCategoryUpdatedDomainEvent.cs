namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCategoryUpdatedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
