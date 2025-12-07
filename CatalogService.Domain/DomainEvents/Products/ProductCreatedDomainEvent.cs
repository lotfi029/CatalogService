namespace CatalogService.Domain.DomainEvents.Products;

public sealed record ProductCreatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductActivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductArchivedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDeactivatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductDeletedDomainEvent(Guid Id) : IDomainEvent;
public sealed record ProductCategoryAddedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
public sealed record ProductCategoryUpdatedDomainEvent(Guid Id, Guid CategoryId, bool IsPrimary) : IDomainEvent;
public sealed record ProductCategoryRemovedDomainEvent(Guid Id, Guid CategoryId) : IDomainEvent;
public sealed record ProductAttributeAddedDomainEvent(Guid Id, Guid AttributeId) : IDomainEvent;
public sealed record ProductVariantAddedDomainEvent(Guid Id, Guid VariantAttributeId) : IDomainEvent;
