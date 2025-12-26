namespace CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

public sealed record CategoryVariantAddedBulkDomainEvent(Guid Id) : IDomainEvent;
