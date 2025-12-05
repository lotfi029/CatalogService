namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductActivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductArchivedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDeactivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDeletedDomainEvent(Guid Id) : IDomainEvent;
