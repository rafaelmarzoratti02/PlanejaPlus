namespace Planeja_.Domain.Entities;

/// <summary>
/// Shared identity, creation audit, and soft-delete state for domain entities.
/// Business rules and invariants belong on concrete types, not here.
/// </summary>
public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    protected EntityBase()
    {
    }

    protected void InitializeIdentity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
        DeletedAt = null;
    }

    protected void ApplySoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
