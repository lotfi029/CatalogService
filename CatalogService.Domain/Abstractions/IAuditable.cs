namespace CatalogService.Domain.Abstractions;

public interface IAuditable
{
    string CreatedBy { get; }
    DateTime CreatedAt { get; }
    string? LastUpdatedBy { get; }
    DateTime? LastUpdatedAt { get; }
    bool IsDeleted { get; }
    string? DeletedBy { get; }
    DateTime? DeletedAt { get; }
    void SetCreationAudit(string createdBy);
    void SetUpdateAudit(string updatedBy);
    void SetDeletionAudit(string deletedBy);
}
