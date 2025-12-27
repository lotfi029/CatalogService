namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
