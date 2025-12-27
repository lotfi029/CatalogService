namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeOptionsUpdatedDomainEvent(Guid Id) : IDomainEvent;
