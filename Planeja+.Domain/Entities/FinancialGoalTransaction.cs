using Planeja_.Domain.Enums;
using Planeja_.Domain.Exceptions;

namespace Planeja_.Domain.Entities;

public class FinancialGoalTransaction
{
    public Guid Id { get; private set; }
    public Guid FinancialGoalId { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionTypeEnum Type { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private FinancialGoalTransaction() { }

    public FinancialGoalTransaction(
        Guid financialGoalId,
        decimal amount,
        TransactionTypeEnum type,
        DateTime date,
        string? description = null)
    {
        ValidateAmount(amount);
        ValidateDate(date);
        ValidateType(type);

        Id = Guid.NewGuid();
        FinancialGoalId = financialGoalId;
        Amount = amount;
        Type = type;
        Date = date;
        Description = description;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("Transaction is already deleted.");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
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

    private static void ValidateType(TransactionTypeEnum type)
    {
        if (!Enum.IsDefined(typeof(TransactionTypeEnum), type))
            throw new DomainException("Invalid transaction type.");
    }
}
