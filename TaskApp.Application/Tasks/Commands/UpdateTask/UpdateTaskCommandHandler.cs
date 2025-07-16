using MediatR;
using Microsoft.Extensions.Logging;
using TaskApp.Application.Common.Cache;
using TaskApp.Infrastructure.Caching;
using TaskApp.Infrastructure.Persistence;

namespace TaskApp.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, bool>
{
    private readonly TaskAppDbContext _db;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;

    public UpdateTaskCommandHandler(TaskAppDbContext db, IRedisCacheService cache, ILogger<UpdateTaskCommandHandler> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _db.Tasks.FindAsync(new { request.Id }, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Task not found: {Id}", request.Id);
            return false;
        }

        task.Update(request.Title, request.Description, request.DueDate);
        await _db.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(TaskCacheKeys.ById(task.Id));
        _logger.LogInformation("Cache invalidated for task {Id}", task.Id);

        return true;
    }
}