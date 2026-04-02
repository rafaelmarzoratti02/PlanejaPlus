using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;
using Planeja_.Domain.Entities;
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

    public async Task<FinancialGoalResponse?> GetByIdAsync(Guid id)
    {
        var goal = await _repository.GetByIdAsync(id);
        return goal is null || goal.IsDeleted ? null : MapToResponse(goal);
    }

    public async Task<IEnumerable<FinancialGoalResponse>> GetAllAsync()
    {
        var goals = await _repository.GetAllAsync();
        return goals.Select(MapToResponse);
    }

    public async Task<FinancialGoalResponse> CreateAsync(CreateFinancialGoalRequest request)
    {
        var goal = new FinancialGoal(
            request.Name,
            request.TargetAmount,
            request.Deadline,
            request.Description);

        await _repository.AddAsync(goal);
        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse> UpdateAsync(Guid id, UpdateFinancialGoalRequest request)
    {
        var goal = await GetOrThrowAsync(id);

        goal.Update(
            request.Name,
            request.TargetAmount,
            request.Deadline,
            request.Description);

        await _repository.UpdateAsync(goal);
        return MapToResponse(goal);
    }

    public async Task DeleteAsync(Guid id)
    {
        var goal = await GetOrThrowAsync(id);
        goal.Delete();
        await _repository.UpdateAsync(goal);
    }

    public async Task<FinancialGoalResponse> CompleteAsync(Guid id)
    {
        var goal = await GetOrThrowAsync(id);
        goal.Complete();
        await _repository.UpdateAsync(goal);
        return MapToResponse(goal);
    }

    public async Task<FinancialGoalResponse> CancelAsync(Guid id)
    {
        var goal = await GetOrThrowAsync(id);
        goal.Cancel();
        await _repository.UpdateAsync(goal);
        return MapToResponse(goal);
    }

    public async Task<TransactionResponse> AddTransactionAsync(Guid goalId, AddTransactionRequest request)
    {
        var goal = await GetOrThrowAsync(goalId);

        var transaction = goal.AddTransaction(
            request.Amount,
            request.Type,
            request.Date,
            request.Description);

        await _repository.UpdateAsync(goal);
        return MapToTransactionResponse(transaction);
    }

    public async Task RemoveTransactionAsync(Guid goalId, Guid transactionId)
    {
        var goal = await GetOrThrowAsync(goalId);
        goal.RemoveTransaction(transactionId);
        await _repository.UpdateAsync(goal);
    }

    private async Task<FinancialGoal> GetOrThrowAsync(Guid id)
    {
        var goal = await _repository.GetByIdAsync(id);

        if (goal is null || goal.IsDeleted)
            throw new DomainException($"Financial goal with id '{id}' not found.");

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
