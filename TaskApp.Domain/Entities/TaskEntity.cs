
using TaskApp.Domain.Abstractions;
using TaskApp.Domain.Enums;

namespace TaskApp.Domain.Entities;

public class TaskEntity : IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; private set; }
    public TaskStatusEnum Status { get; private set; } = TaskStatusEnum.Pending;

    private TaskEntity() { }

    public TaskEntity(string title, string? description, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    public void Update(string title, string? description, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    public void MarkInProgress() => Status = TaskStatusEnum.InProgress;
    public void MarkCompleted() => Status = TaskStatusEnum.Completed;
}