namespace TaskApp.Contracts.Messaging;
public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(string topic, T message);
}