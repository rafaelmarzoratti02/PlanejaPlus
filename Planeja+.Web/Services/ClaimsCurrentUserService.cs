using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Planeja_.Application.Abstractions;

namespace Planeja_.Web.Services;

public sealed class ClaimsCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(claim, out var id)
                ? id
                : Guid.Empty;
        }
    }
}
