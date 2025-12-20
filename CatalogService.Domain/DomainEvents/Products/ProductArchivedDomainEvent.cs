namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductArchivedDomainEvent(Guid Id) : IDomainEvent;
