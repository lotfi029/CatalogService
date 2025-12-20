namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
