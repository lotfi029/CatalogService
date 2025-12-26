namespace CatalogService.Domain.DomainEvents.Categories.CategoryVariants;

public sealed record CategoryVariantUpdatedDomainEvent(Guid Id, Guid VariantAttributeId) : IDomainEvent;
