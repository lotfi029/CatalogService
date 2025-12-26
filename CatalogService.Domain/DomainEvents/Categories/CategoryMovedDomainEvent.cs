namespace CatalogService.Domain.DomainEvents.Categories;

public sealed record CategoryMovedDomainEvent(Guid Id) : IDomainEvent;
