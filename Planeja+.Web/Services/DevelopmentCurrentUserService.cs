using Planeja_.Application.Abstractions;

namespace Planeja_.Web.Services;

public sealed class DevelopmentCurrentUserService : ICurrentUserService
{
    public static readonly Guid DefaultUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public Guid UserId => DefaultUserId;
}
