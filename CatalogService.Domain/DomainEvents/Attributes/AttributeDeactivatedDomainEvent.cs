namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeDeactivatedDomainEvent(Guid Id) : IDomainEvent;
