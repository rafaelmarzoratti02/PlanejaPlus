using Planeja_.Domain.Entities;

namespace Planeja_.Domain.Repositories;

public interface IFinancialGoalRepository
{
    Task<FinancialGoal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialGoal>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(FinancialGoal goal, CancellationToken cancellationToken = default);
    Task UpdateAsync(FinancialGoal goal, CancellationToken cancellationToken = default);
}
