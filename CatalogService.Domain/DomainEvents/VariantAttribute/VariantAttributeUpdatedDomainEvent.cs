namespace CatalogService.Domain.DomainEvents.VariantAttribute;

public sealed record VariantAttributeUpdatedDomainEvent(Guid Id) : IDomainEvent;
