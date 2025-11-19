using CatalogService.Domain.Abstractions;

namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryCreatedDomainEvent(Guid CategoryId) : IDomainEvent;
public sealed record CategoryUpdatedDomainEvent : IDomainEvent;
public sealed record CategoryDeletedDomainEvent : IDomainEvent;
public sealed record VariantCategoryCreatedDomainEvent : IDomainEvent;