namespace CatalogService.Domain.Entities;

public interface IDomainEvent
{
    Guid DomainId { get; }
    DateTime OccurredOn { get; }
}