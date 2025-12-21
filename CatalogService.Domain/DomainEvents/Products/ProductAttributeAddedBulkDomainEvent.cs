namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductAttributeAddedBulkDomainEvent(Guid Id) : IDomainEvent;
