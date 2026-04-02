namespace Planeja_.Application.DTOs.FinancialGoals;

public record CreateFinancialGoalRequest(
    string Name,
    decimal TargetAmount,
    DateTime Deadline,
    string? Description = null);
