using Microsoft.Extensions.DependencyInjection;
using Planeja_.Domain.Repositories;
using Planeja_.Infrastructure.Persistence;

namespace Planeja_.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFinancialGoalRepository, InMemoryFinancialGoalRepository>();

        return services;
    }
}
