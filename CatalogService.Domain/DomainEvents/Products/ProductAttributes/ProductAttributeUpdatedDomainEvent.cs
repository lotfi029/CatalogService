namespace CatalogService.Domain.DomainEvents.Products.ProductAttributes;

public sealed record ProductAttributeUpdatedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
