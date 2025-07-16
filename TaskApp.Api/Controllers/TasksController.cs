using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskApp.Application.Tasks.Commands.CreateTask;
using TaskApp.Application.Tasks.Commands.DeleteTask;
using TaskApp.Application.Tasks.Commands.UpdateTask;
using TaskApp.Application.Tasks.DTOs;
using TaskApp.Application.Tasks.Queries.GetTaskById;

namespace TaskApp.Api.Controllers;

/// <summary>
/// Manages task-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<object>> CreateAsync(
        [FromBody] CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var taskId = await _mediator.Send(command, cancellationToken);
        _logger.LogInformation("Created new task with ID {TaskId}", taskId);

        return Ok(taskId);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTaskByIdQuery(id);
        var task = await _mediator.Send(query, cancellationToken);

        return task is null ? NotFound() : Ok(task);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Route ID and body ID must match");

        var success = await _mediator.Send(command, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
        return success ? NoContent() : NotFound();
    }
}