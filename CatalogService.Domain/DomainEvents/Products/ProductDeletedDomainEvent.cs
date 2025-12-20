namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductDeletedDomainEvent(Guid Id) : IDomainEvent;
