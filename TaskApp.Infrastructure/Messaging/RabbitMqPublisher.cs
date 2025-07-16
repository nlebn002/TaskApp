using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using TaskApp.Contracts.Messaging;

public class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
{
    private readonly IModel _channel;

    public RabbitMqPublisher(IConnection connection)
    {
        _channel = connection.CreateModel();
    }

    public Task PublishAsync<T>(string topic, T message)
    {
        _channel.ExchangeDeclare(topic, ExchangeType.Fanout, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        _channel.BasicPublish(exchange: topic, routingKey: "", basicProperties: null, body: body);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}