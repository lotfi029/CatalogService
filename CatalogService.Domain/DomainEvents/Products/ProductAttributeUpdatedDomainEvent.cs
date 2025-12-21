namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductAttributeUpdatedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
