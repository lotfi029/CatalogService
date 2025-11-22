namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryCreatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record CategoryMovedDomainEvent(Guid Id) : IDomainEvent;
public sealed record CategoryDetailsUpdatedDomainEvent(Guid Id) : IDomainEvent;
public sealed record CategoryDeletedDomainEvent : IDomainEvent;
public sealed record VariantCategoryCreatedDomainEvent : IDomainEvent;