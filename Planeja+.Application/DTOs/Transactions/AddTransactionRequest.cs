using Planeja_.Domain.Enums;

namespace Planeja_.Application.DTOs.Transactions;

public record AddTransactionRequest(
    decimal Amount,
    TransactionTypeEnum Type,
    DateTime Date,
    string? Description = null);
