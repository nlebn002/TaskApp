using MediatR;
using TaskApp.Application.Tasks.DTOs;

namespace TaskApp.Application.Tasks.Queries.GetTaskById;
public record GetTaskByIdQuery(Guid TaskId) : IRequest<TaskDto>;
