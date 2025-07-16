using MediatR;
using TaskApp.Domain.Entities;
using TaskApp.Infrastructure.Persistence;

namespace TaskApp.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly TaskAppDbContext _db;

    public CreateTaskCommandHandler(TaskAppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskEntity(request.Title, request.Description, request.DueDate);

        await _db.Tasks.AddAsync(task, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}