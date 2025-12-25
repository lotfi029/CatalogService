namespace CatalogService.Domain.DomainEvents.Products.ProductAttributes;
public sealed record ProductAttributeAddedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
