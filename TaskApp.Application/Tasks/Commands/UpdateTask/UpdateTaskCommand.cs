using MediatR;

namespace TaskApp.Application.Tasks.Commands.UpdateTask;
public record UpdateTaskCommand(
    Guid Id,
    string Title,
    string? Description,
    DateTime? DueDate
) : IRequest<bool>;