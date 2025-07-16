using MediatR;
using Microsoft.Extensions.Logging;
using TaskApp.Application.Common.Cache;
using TaskApp.Application.Tasks.Commands.DeleteTask;
using TaskApp.Infrastructure.Caching;
using TaskApp.Infrastructure.Persistence;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly TaskAppDbContext _db;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<DeleteTaskCommandHandler> _logger;

    public DeleteTaskCommandHandler(TaskAppDbContext db, IRedisCacheService cache, ILogger<DeleteTaskCommandHandler> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _db.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Task not found: {Id}", request.Id);
            return false;
        }

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(TaskCacheKeys.ById(task.Id));
        _logger.LogInformation("Cache invalidated after delete: {Id}", task.Id);

        return true;
    }
}