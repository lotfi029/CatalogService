namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductDeactivatedDomainEvent(Guid Id) : IDomainEvent;
