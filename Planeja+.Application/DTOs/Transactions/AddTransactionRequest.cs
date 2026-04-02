namespace Planeja_.Application.DTOs.Transactions;

public record AddTransactionRequest(
    decimal Amount,
    int Type,
    DateTime Date,
    string? Description = null);
