using MediatR;

namespace TaskApp.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    DateTime? DueDate
) : IRequest<Guid>;