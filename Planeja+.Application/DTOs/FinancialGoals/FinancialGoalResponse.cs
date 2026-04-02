using Planeja_.Application.DTOs.Transactions;

namespace Planeja_.Application.DTOs.FinancialGoals;

public record FinancialGoalResponse(
    Guid Id,
    string Name,
    decimal TargetAmount,
    decimal CurrentAmount,
    string Status,
    DateTime Deadline,
    string? Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyCollection<TransactionResponse> Transactions);
