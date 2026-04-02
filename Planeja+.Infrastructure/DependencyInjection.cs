using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Planeja_.Application.Abstractions;
using Planeja_.Application.Services;
using Planeja_.Domain.Repositories;
using Planeja_.Infrastructure.Identity;
using Planeja_.Infrastructure.Persistence;

namespace Planeja_.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFinancialGoalRepository, InMemoryFinancialGoalRepository>();

        services.AddDbContext<AuthDbContext>(options =>
            options.UseInMemoryDatabase("PlanejaPlusAuth"));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
