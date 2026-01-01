namespace CatalogService.Domain.DomainService.ProductVariantValue;

public sealed record ProductVariantValueDeletedDomainEvent(Guid Id, Guid ProductId, Guid VariantAttributeId) : IDomainEvent;