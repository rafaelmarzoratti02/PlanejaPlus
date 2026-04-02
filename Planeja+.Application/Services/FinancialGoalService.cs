using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;
using Planeja_.Application.Exceptions;
using Planeja_.Domain.Entities;
using Planeja_.Domain.Enums;
using Planeja_.Domain.Exceptions;
using Planeja_.Domain.Repositories;

namespace Planeja_.Application.Services;

public sealed class FinancialGoalService : IFinancialGoalService
{
    private readonly IFinancialGoalRepository _repository;

    public FinancialGoalService(IFinancialGoalRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinancialGoalResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await _repository.GetByIdAsync(id, cancellationToken);
        return goal is null || goal.IsDeleted ? null : MapToResponse(goal);
    }

    public async Task<IEnumerable<FinancialGoalResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var goals = await _repository.GetAllAsync(cancellationToken);
        return goals.Select(MapToResponse);
    }

    public async Task<FinancialGoalResponse> CreateAsync(CreateFinancialGoalRequest request, CancellationToken cancellationToken = default)
    {
        var goal = new FinancialGoal(
            request.Name,
            request.TargetAmount,
            request.Deadline,
            request.Description);

        await _repository.AddAsync(goal, cancellationToken);
        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse> UpdateAsync(Guid id, UpdateFinancialGoalRequest request, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(id, cancellationToken);

        goal.Update(
            request.Name,
            request.TargetAmount,
            request.Deadline,
            request.Description);

        await _repository.UpdateAsync(goal, cancellationToken);
        return MapToResponse(goal);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(id, cancellationToken);
        goal.Delete();
        await _repository.UpdateAsync(goal, cancellationToken);
    }

    public async Task<FinancialGoalResponse> CompleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(id, cancellationToken);
        goal.Complete();
        await _repository.UpdateAsync(goal, cancellationToken);
        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(id, cancellationToken);
        goal.Cancel();
        await _repository.UpdateAsync(goal, cancellationToken);
        return MapToResponse(goal);
    }

    public async Task<TransactionResponse> AddTransactionAsync(Guid goalId, AddTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(goalId, cancellationToken);

        if (!Enum.IsDefined(typeof(TransactionType), request.Type))
            throw new DomainException($"Invalid transaction type: {request.Type}. Use 1 (Deposit) or 2 (Withdraw).");

        var transactionType = (TransactionType)request.Type;

        var transaction = goal.AddTransaction(
            request.Amount,
            transactionType,
            request.Date,
            request.Description);

        await _repository.UpdateAsync(goal, cancellationToken);
        return MapToTransactionResponse(transaction);
    }

    public async Task RemoveTransactionAsync(Guid goalId, Guid transactionId, CancellationToken cancellationToken = default)
    {
        var goal = await GetOrThrowAsync(goalId, cancellationToken);
        goal.RemoveTransaction(transactionId);
        await _repository.UpdateAsync(goal, cancellationToken);
    }

    private async Task<FinancialGoal> GetOrThrowAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var goal = await _repository.GetByIdAsync(id, cancellationToken);

        if (goal is null || goal.IsDeleted)
            throw new NotFoundException(nameof(FinancialGoal), id);

        return goal;
    }

    private static FinancialGoalResponse MapToResponse(FinancialGoal goal)
    {
        return new FinancialGoalResponse(
            goal.Id,
            goal.Name,
            goal.TargetAmount,
            goal.CurrentAmount,
            goal.Status.ToString(),
            goal.Deadline,
            goal.Description,
            goal.CreatedAt,
            goal.UpdatedAt,
            goal.Transactions
                .Where(t => !t.IsDeleted)
                .Select(MapToTransactionResponse)
                .ToList()
                .AsReadOnly());
    }

    private static TransactionResponse MapToTransactionResponse(FinancialGoalTransaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.Amount,
            transaction.Type.ToString(),
            transaction.Date,
            transaction.Description,
            transaction.CreatedAt);
    }
}
