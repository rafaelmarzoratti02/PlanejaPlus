using Planeja_.Application.Abstractions;

namespace Planeja_.Web.Services;

/// <summary>
/// Returns a fixed user id until JWT auth (#24–#26) supplies real identities.
/// Keeps existing goals scoped consistently across requests.
/// </summary>
public sealed class DevelopmentCurrentUserService : ICurrentUserService
{
    /// <summary>Stable dev/anonymous tenant id (not <see cref="Guid.Empty"/>).</summary>
    public static readonly Guid DefaultUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public Guid UserId => DefaultUserId;
}
