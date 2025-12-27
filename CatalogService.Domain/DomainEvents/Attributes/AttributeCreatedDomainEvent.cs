namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeCreatedDomainEvent(Guid Id) : IDomainEvent;
