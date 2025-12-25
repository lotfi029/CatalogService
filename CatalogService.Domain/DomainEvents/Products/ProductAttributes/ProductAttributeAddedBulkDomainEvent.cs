namespace CatalogService.Domain.DomainEvents.Products.ProductAttributes;

public sealed record ProductAttributeAddedBulkDomainEvent(Guid Id) : IDomainEvent;
