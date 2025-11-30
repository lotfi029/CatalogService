namespace CatalogService.Domain.DomainEvents.VariantAttribute;

public sealed record VariantAttributeCreatedDomainEvent(Guid Id) : IDomainEvent;