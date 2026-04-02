using System.Collections.Concurrent;
using Planeja_.Domain.Entities;
using Planeja_.Domain.Repositories;

namespace Planeja_.Infrastructure.Persistence;

public sealed class InMemoryFinancialGoalRepository : IFinancialGoalRepository
{
    private readonly ConcurrentDictionary<Guid, FinancialGoal> _store = new();

    public Task<FinancialGoal?> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var goal);
        return Task.FromResult(goal);
    }

    public Task<IEnumerable<FinancialGoal>> GetAllAsync()
    {
        IEnumerable<FinancialGoal> goals = _store.Values
            .Where(g => !g.IsDeleted)
            .OrderByDescending(g => g.CreatedAt);

        return Task.FromResult(goals);
    }

    public Task AddAsync(FinancialGoal goal)
    {
        if (!_store.TryAdd(goal.Id, goal))
            throw new InvalidOperationException($"A goal with id '{goal.Id}' already exists.");

        return Task.CompletedTask;
    }

    public Task UpdateAsync(FinancialGoal goal)
    {
        _store[goal.Id] = goal;
        return Task.CompletedTask;
    }
}
