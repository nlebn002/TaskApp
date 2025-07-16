namespace TaskApp.Contracts.Events.Tasks;
public record TaskCreatedEvent(
    Guid TaskId,
    string Title,
    DateTime CreatedAt
);