using Planeja_.Domain.Enums;
using Planeja_.Domain.Exceptions;

namespace Planeja_.Domain.Entities;

public class FinancialGoalTransaction : EntityBase
{
    public Guid FinancialGoalId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }

    private FinancialGoalTransaction()
    {
    }

    public FinancialGoalTransaction(
        Guid financialGoalId,
        decimal amount,
        TransactionType type,
        DateTime date,
        string? description = null)
    {
        ValidateAmount(amount);
        ValidateDate(date);
        ValidateType(type);

        InitializeIdentity();

        FinancialGoalId = financialGoalId;
        Amount = amount;
        Type = type;
        Date = date;
        Description = description;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("Transaction is already deleted.");

        ApplySoftDelete();
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Transaction amount must be greater than zero.");

        if (decimal.Round(amount, 2) != amount)
            throw new DomainException("Transaction amount must have at most 2 decimal places.");
    }

    private static void ValidateDate(DateTime date)
    {
        if (date.Date > DateTime.UtcNow.Date)
            throw new DomainException("Transaction date cannot be in the future.");
    }

    private static void ValidateType(TransactionType type)
    {
        if (!Enum.IsDefined(typeof(TransactionType), type))
            throw new DomainException("Invalid transaction type.");
    }
}
