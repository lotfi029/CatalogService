namespace CatalogService.Domain.Abstractions;

public abstract class AuditableEntity : Entity, IAuditable
{
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public string? LastUpdatedBy { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }

    public bool IsActive { get; private set; } = true;

    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    protected AuditableEntity() : base() { }
    protected AuditableEntity(Guid id) : base(id) { }

    public virtual void SetCreationAudit(string createdBy)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
    }

    public virtual void SetDeletionAudit(string deletedBy)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public virtual void SetUpdateAudit(string updatedBy)
    {
        LastUpdatedAt = DateTime.UtcNow;
        LastUpdatedBy = updatedBy;
    }

    protected virtual void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        Deactive();

    }
    protected virtual void Active()
    {
        if (IsActive)
            return;

        IsActive = true;
    }
    protected virtual void Deactive()
    {
        if (!IsActive)
            return;

        IsActive = false;
    }
}
