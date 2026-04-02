using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;

namespace Planeja_.Application.Services;

public interface IFinancialGoalService
{
    Task<FinancialGoalResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<FinancialGoalResponse>> GetAllAsync();
    Task<FinancialGoalResponse> CreateAsync(CreateFinancialGoalRequest request);
    Task<FinancialGoalResponse> UpdateAsync(Guid id, UpdateFinancialGoalRequest request);
    Task DeleteAsync(Guid id);
    Task<FinancialGoalResponse> CompleteAsync(Guid id);
    Task<FinancialGoalResponse> CancelAsync(Guid id);
    Task<TransactionResponse> AddTransactionAsync(Guid goalId, AddTransactionRequest request);
    Task RemoveTransactionAsync(Guid goalId, Guid transactionId);
}
