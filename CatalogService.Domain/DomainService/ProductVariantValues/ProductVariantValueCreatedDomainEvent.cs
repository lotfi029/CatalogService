namespace CatalogService.Domain.DomainService.ProductVariantValues;

public sealed record ProductVariantValueCreatedDomainEvent(Guid Id, Guid ProductId, Guid VariantAttributeId) : IDomainEvent;