namespace Planeja_.Domain.Entities;

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
