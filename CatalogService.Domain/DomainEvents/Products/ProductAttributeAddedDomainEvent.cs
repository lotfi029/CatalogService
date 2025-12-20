namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductAttributeAddedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
