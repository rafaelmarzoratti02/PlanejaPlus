using Microsoft.AspNetCore.Mvc;
using Planeja_.Application.DTOs.FinancialGoals;
using Planeja_.Application.DTOs.Transactions;
using Planeja_.Application.Services;

namespace Planeja_.Controllers;

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
    public async Task<IActionResult> GetAll()
    {
        var goals = await _service.GetAllAsync();
        return Ok(goals);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var goal = await _service.GetByIdAsync(id);
        return goal is null ? NotFound() : Ok(goal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFinancialGoalRequest request)
    {
        var goal = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateFinancialGoalRequest request)
    {
        var goal = await _service.UpdateAsync(id, request);
        return Ok(goal);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var goal = await _service.CompleteAsync(id);
        return Ok(goal);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var goal = await _service.CancelAsync(id);
        return Ok(goal);
    }

    [HttpPost("{id:guid}/transactions")]
    public async Task<IActionResult> AddTransaction(Guid id, [FromBody] AddTransactionRequest request)
    {
        var transaction = await _service.AddTransactionAsync(id, request);
        return Created($"api/financial-goals/{id}/transactions/{transaction.Id}", transaction);
    }

    [HttpDelete("{id:guid}/transactions/{transactionId:guid}")]
    public async Task<IActionResult> RemoveTransaction(Guid id, Guid transactionId)
    {
        await _service.RemoveTransactionAsync(id, transactionId);
        return NoContent();
    }
}
