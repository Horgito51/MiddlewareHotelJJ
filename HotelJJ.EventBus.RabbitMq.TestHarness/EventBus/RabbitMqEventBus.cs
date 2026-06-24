using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;

public sealed class RabbitMqEventBus : IEventBus
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = false
    };

    private readonly RabbitMqConnection _connection;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly RabbitMqOptions _options;

    public RabbitMqEventBus(
        RabbitMqConnection connection,
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqEventBus> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        string routingKey,
        CancellationToken cancellationToken = default)
        where TEvent : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent, JsonOptions));

        using var channel = _connection.CreateChannel();
        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.DeliveryMode = 2;
        properties.MessageId = TryGetProperty(integrationEvent, "EventId") ?? Guid.NewGuid().ToString("D");
        properties.CorrelationId = TryGetProperty(integrationEvent, "CorrelationId");
        properties.Type = TryGetProperty(integrationEvent, "EventType");
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        channel.BasicPublish(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body);

        _logger.LogInformation(
            "Evento publicado. Exchange={Exchange}, RoutingKey={RoutingKey}, MessageId={MessageId}, CorrelationId={CorrelationId}",
            _options.ExchangeName,
            routingKey,
            properties.MessageId,
            properties.CorrelationId);

        return Task.CompletedTask;
    }

    private static string? TryGetProperty<TEvent>(TEvent integrationEvent, string propertyName)
        where TEvent : class
    {
        var property = typeof(TEvent).GetProperty(propertyName);
        var value = property?.GetValue(integrationEvent);

        return value switch
        {
            null => null,
            Guid guid => guid.ToString("D"),
            DateTime dateTime => dateTime.ToUniversalTime().ToString("O"),
            _ => value.ToString()
        };
    }
}

