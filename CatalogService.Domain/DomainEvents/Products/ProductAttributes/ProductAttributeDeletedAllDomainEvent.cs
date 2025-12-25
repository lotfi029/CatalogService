namespace CatalogService.Domain.DomainEvents.Products.ProductAttributes;

public sealed record ProductAttributeDeletedAllDomainEvent(Guid Id) : IDomainEvent;