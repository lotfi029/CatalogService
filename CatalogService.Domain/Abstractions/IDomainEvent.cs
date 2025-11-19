namespace CatalogService.Domain.Abstractions;

public interface IDomainEvent
{
    Guid DomainId { get; } 
    DateTime OccurredOn { get; }
}