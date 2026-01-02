namespace CatalogService.Domain.DomainService.ProductVariantValues;

public sealed record ProductVariantValueDeletedDomainEvent(Guid Id, Guid ProductId, Guid VariantAttributeId) : IDomainEvent;