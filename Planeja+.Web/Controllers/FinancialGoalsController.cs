using Microsoft.AspNetCore.Mvc;
using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;
using Planeja_.Application.Services;

namespace Planeja_.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinancialGoalsController : ControllerBase
{
    private readonly IFinancialGoalService _service;

    public FinancialGoalsController(IFinancialGoalService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var goals = await _service.GetAllAsync(cancellationToken);
        return Ok(goals);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var goal = await _service.GetByIdAsync(id, cancellationToken);
        return goal is null ? NotFound() : Ok(goal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFinancialGoalRequest request, CancellationToken cancellationToken)
    {
        var goal = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFinancialGoalRequest request, CancellationToken cancellationToken)
    {
        var goal = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(goal);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var goal = await _service.CompleteAsync(id, cancellationToken);
        return Ok(goal);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var goal = await _service.CancelAsync(id, cancellationToken);
        return Ok(goal);
    }

    [HttpPost("{id:guid}/transactions")]
    public async Task<IActionResult> AddTransaction(Guid id, [FromBody] AddTransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _service.AddTransactionAsync(id, request, cancellationToken);
        return Created($"api/financial-goals/{id}/transactions/{transaction.Id}", transaction);
    }

    [HttpDelete("{id:guid}/transactions/{transactionId:guid}")]
    public async Task<IActionResult> RemoveTransaction(Guid id, Guid transactionId, CancellationToken cancellationToken)
    {
        await _service.RemoveTransactionAsync(id, transactionId, cancellationToken);
        return NoContent();
    }
}
