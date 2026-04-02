using System.Collections.Concurrent;
using Planeja_.Domain.Entities;
using Planeja_.Domain.Repositories;

namespace Planeja_.Infrastructure.Persistence;

public sealed class InMemoryFinancialGoalRepository : IFinancialGoalRepository
{
    private static readonly ConcurrentDictionary<Guid, FinancialGoal> Store = new();

    public Task<FinancialGoal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Store.TryGetValue(id, out var goal);
        return Task.FromResult(goal);
    }

    public Task<IEnumerable<FinancialGoal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<FinancialGoal> goals = Store.Values
            .Where(g => !g.IsDeleted)
            .OrderByDescending(g => g.CreatedAt);

        return Task.FromResult(goals);
    }

    public Task AddAsync(FinancialGoal goal, CancellationToken cancellationToken = default)
    {
        if (!Store.TryAdd(goal.Id, goal))
            throw new InvalidOperationException($"A goal with id '{goal.Id}' already exists.");

        return Task.CompletedTask;
    }

    public Task UpdateAsync(FinancialGoal goal, CancellationToken cancellationToken = default)
    {
        Store[goal.Id] = goal;
        return Task.CompletedTask;
    }
}
