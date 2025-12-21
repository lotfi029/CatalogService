namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
