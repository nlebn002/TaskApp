using MediatR;

namespace TaskApp.Application.Tasks.Commands.DeleteTask;
public record DeleteTaskCommand(Guid Id) : IRequest<bool>;
