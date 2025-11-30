namespace CatalogService.Domain.DomainEvents.VariantAttribute;

public sealed record VariantAttributeNameUpdatedDomainEvent(Guid Id) : IDomainEvent;