namespace Planeja_.Domain.Entities;

/// <summary>
/// Aggregate roots (or entities) that track last modification time.
/// Child entities that never update after creation can inherit <see cref="EntityBase"/> only.
/// </summary>
public abstract class AuditableEntityBase : EntityBase
{
    public DateTime? UpdatedAt { get; protected set; }

    protected AuditableEntityBase()
    {
    }

    protected void TouchUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
