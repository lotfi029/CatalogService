namespace CatalogService.Domain.DomainEvents.Products.ProductAttributes;

public sealed record ProductAttributeDeletedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
