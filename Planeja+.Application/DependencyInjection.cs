using Microsoft.Extensions.DependencyInjection;
using Planeja_.Application.Services;

namespace Planeja_.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IFinancialGoalService, FinancialGoalService>();

        return services;
    }
}
