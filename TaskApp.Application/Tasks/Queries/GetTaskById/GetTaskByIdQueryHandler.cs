using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskApp.Application.Tasks.DTOs;
using TaskApp.Infrastructure.Caching;
using TaskApp.Infrastructure.Persistence;

namespace TaskApp.Application.Tasks.Queries.GetTaskById;
public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly TaskAppDbContext _db;
    private readonly IRedisCacheService _cache;
    private readonly ILogger<GetTaskByIdQueryHandler> _logger;

    public GetTaskByIdQueryHandler(
        TaskAppDbContext db,
        IRedisCacheService cache,
        ILogger<GetTaskByIdQueryHandler> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"task:{request.TaskId}";

        var cached = await _cache.GetAsync<TaskDto>(cacheKey);
        if (cached is not null)
        {
            _logger.LogInformation("Cache HIT for {TaskId}", request.TaskId);
            return cached;
        }

        _logger.LogInformation("Cache MISS for {TaskId}", request.TaskId);

        var task = await _db.Tasks
            .AsNoTracking()
            .Where(t => t.Id == request.TaskId)
            .Select(t => new TaskDto(
                t.Id,
                t.Title,
                t.Description,
                t.CreatedAt,
                t.DueDate,
                t.Status.ToString()))
            .FirstOrDefaultAsync(cancellationToken);

        if (task is not null)
            await _cache.SetAsync(cacheKey, task, TimeSpan.FromMinutes(10));

        return task;
    }
}