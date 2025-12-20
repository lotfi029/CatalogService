namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductActivatedDomainEvent(Guid Id) : IDomainEvent;
