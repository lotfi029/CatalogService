namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeActivatedDomainEvent(Guid Id) : IDomainEvent;
