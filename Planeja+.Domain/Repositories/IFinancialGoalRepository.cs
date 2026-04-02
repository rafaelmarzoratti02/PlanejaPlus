using Planeja_.Domain.Entities;

namespace Planeja_.Domain.Repositories;

public interface IFinancialGoalRepository
{
    Task<FinancialGoal?> GetByIdAsync(Guid id);
    Task<IEnumerable<FinancialGoal>> GetAllAsync();
    Task AddAsync(FinancialGoal goal);
    Task UpdateAsync(FinancialGoal goal);
}
