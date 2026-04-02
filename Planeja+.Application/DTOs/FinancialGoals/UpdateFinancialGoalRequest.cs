namespace Planeja_.Application.DTOs.FinancialGoals;

public record UpdateFinancialGoalRequest(
    string Name,
    decimal TargetAmount,
    DateTime Deadline,
    string? Description = null);
