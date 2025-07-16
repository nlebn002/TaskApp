namespace TaskApp.Contracts.Events.Tasks;

public record TaskUpdatedEvent(
    Guid TaskId,
    string Title,
    DateTime UpdatedAt
);