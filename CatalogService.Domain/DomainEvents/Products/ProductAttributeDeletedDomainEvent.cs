namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductAttributeDeletedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
