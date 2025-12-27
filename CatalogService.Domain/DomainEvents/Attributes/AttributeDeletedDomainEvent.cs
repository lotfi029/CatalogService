namespace CatalogService.Domain.DomainEvents.Attributes;

public sealed record AttributeDeletedDomainEvent(Guid Id) : IDomainEvent;
