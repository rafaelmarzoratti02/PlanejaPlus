using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;

namespace Planeja_.Application.Services;

public interface IFinancialGoalService
{
    Task<FinancialGoalResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialGoalResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FinancialGoalResponse> CreateAsync(CreateFinancialGoalRequest request, CancellationToken cancellationToken = default);
    Task<FinancialGoalResponse> UpdateAsync(Guid id, UpdateFinancialGoalRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FinancialGoalResponse> CompleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FinancialGoalResponse> CancelAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TransactionResponse> AddTransactionAsync(Guid goalId, AddTransactionRequest request, CancellationToken cancellationToken = default);
    Task RemoveTransactionAsync(Guid goalId, Guid transactionId, CancellationToken cancellationToken = default);
}
