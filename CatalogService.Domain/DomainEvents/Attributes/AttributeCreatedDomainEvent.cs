namespace CatalogService.Domain.DomainEvents.Attributes;

public record AttributeCreatedDomainEvent(Guid Id) : IDomainEvent;
public record AttributeOptionsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public record AttributeDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public record AttributeActivatedDomainEvent(Guid Id) : IDomainEvent;
public record AttributeDeactivatedDomainEvent(Guid Id) : IDomainEvent;
public record AttributeDeletedDomainEvent(Guid Id) : IDomainEvent;
