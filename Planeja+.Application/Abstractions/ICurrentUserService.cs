namespace Planeja_.Application.Abstractions;

/// <summary>
/// Provides the authenticated user's id for application-layer use cases.
/// Web layer supplies the implementation (claims in #26; dev stub until then).
/// </summary>
public interface ICurrentUserService
{
    Guid UserId { get; }
}
