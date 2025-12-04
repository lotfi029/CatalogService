namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeCreatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record AttributeOptionsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record AttributeDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record AttributeActivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record AttributeDeactivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record AttributeDeletedDomainEvent(Guid Id) : IDomainEvent;
