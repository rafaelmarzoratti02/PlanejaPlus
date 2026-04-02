namespace Planeja_.Application.DTOs.Transactions;

public record TransactionResponse(
    Guid Id,
    decimal Amount,
    string Type,
    DateTime Date,
    string? Description,
    DateTime CreatedAt);
