namespace TaskApp.Application.Tasks.DTOs;

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    DateTime CreatedAt,
    DateTime? DueDate,
    string Status
);
