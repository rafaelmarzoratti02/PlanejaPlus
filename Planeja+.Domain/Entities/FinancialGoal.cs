using Planeja_.Domain.Enums;
using Planeja_.Domain.Exceptions;

namespace Planeja_.Domain.Entities;

public class FinancialGoal : AuditableEntityBase
{
    private readonly List<FinancialGoalTransaction> _transactions = new();

    public Guid UserId { get; private set; }
    public string Name { get; private set; } = default!;
    public decimal TargetAmount { get; private set; }
    public FinancialGoalStatus Status { get; private set; }
    public DateTime Deadline { get; private set; }
    public string? Description { get; private set; }

    public IReadOnlyCollection<FinancialGoalTransaction> Transactions => _transactions.AsReadOnly();

    public decimal CurrentAmount => _transactions
        .Where(t => !t.IsDeleted)
        .Sum(t => t.Type == TransactionType.Deposit ? t.Amount : -t.Amount);

    private FinancialGoal()
    {
    }

    public FinancialGoal(Guid userId, string name, decimal targetAmount, DateTime deadline, string? description = null)
    {
        ValidateUserId(userId);
        ValidateName(name);
        ValidateTargetAmount(targetAmount);
        ValidateDeadline(deadline);

        InitializeIdentity();

        UserId = userId;
        Name = name;
        TargetAmount = targetAmount;
        Status = FinancialGoalStatus.Active;
        Deadline = deadline;
        Description = description;
    }

    public void Update(string name, decimal targetAmount, DateTime deadline, string? description = null)
    {
        EnsureNotDeleted();

        ValidateName(name);
        ValidateTargetAmount(targetAmount);

        Name = name;
        TargetAmount = targetAmount;
        Deadline = deadline;
        Description = description;
        TouchUpdated();
    }

    public FinancialGoalTransaction AddTransaction(decimal amount, TransactionType type, DateTime date, string? description = null)
    {
        EnsureNotDeleted();

        if (Status != FinancialGoalStatus.Active)
            throw new DomainException("Cannot add transactions to a non-active goal.");

        var transaction = new FinancialGoalTransaction(Id, amount, type, date, description);
        _transactions.Add(transaction);
        return transaction;
    }

    public void RemoveTransaction(Guid transactionId)
    {
        EnsureNotDeleted();

        var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId)
            ?? throw new DomainException($"Transaction with id '{transactionId}' not found.");

        transaction.Delete();
    }

    public void Complete()
    {
        EnsureNotDeleted();

        if (Status != FinancialGoalStatus.Active)
            throw new DomainException("Only active goals can be completed.");

        Status = FinancialGoalStatus.Completed;
        TouchUpdated();
    }

    public void Cancel()
    {
        EnsureNotDeleted();

        if (Status != FinancialGoalStatus.Active)
            throw new DomainException("Only active goals can be cancelled.");

        Status = FinancialGoalStatus.Cancelled;
        TouchUpdated();
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new DomainException("Goal is already deleted.");

        ApplySoftDelete();
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Cannot modify a deleted goal.");
    }

    private static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User id is required.");
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Goal name is required.");

        if (name.Length > 200)
            throw new DomainException("Goal name must not exceed 200 characters.");
    }

    private static void ValidateTargetAmount(decimal targetAmount)
    {
        if (targetAmount <= 0)
            throw new DomainException("Target amount must be greater than zero.");

        if (decimal.Round(targetAmount, 2) != targetAmount)
            throw new DomainException("Target amount must have at most 2 decimal places.");
    }

    private static void ValidateDeadline(DateTime deadline)
    {
        if (deadline.Date < DateTime.UtcNow.Date)
            throw new DomainException("Deadline cannot be in the past.");
    }
}
