using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;

public sealed class RabbitMqConnection : IDisposable
{
    private readonly ILogger<RabbitMqConnection> _logger;
    private readonly RabbitMqOptions _options;
    private readonly object _syncRoot = new();
    private IConnection? _connection;

    public RabbitMqConnection(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqConnection> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public IConnection GetConnection()
    {
        if (_connection is { IsOpen: true })
        {
            return _connection;
        }

        lock (_syncRoot)
        {
            if (_connection is { IsOpen: true })
            {
                return _connection;
            }

            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                DispatchConsumersAsync = false
            };

            _connection = factory.CreateConnection("hoteljj-eventbus-test-harness");
            _logger.LogInformation(
                "RabbitMQ conectado en {Host}:{Port}, vhost {VirtualHost}",
                _options.HostName,
                _options.Port,
                _options.VirtualHost);

            using var channel = _connection.CreateModel();
            DeclareTopology(channel);

            return _connection;
        }
    }

    public IModel CreateChannel()
    {
        return GetConnection().CreateModel();
    }

    private void DeclareTopology(IModel channel)
    {
        channel.ExchangeDeclare(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        channel.ExchangeDeclare(
            exchange: _options.DeadLetterExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        channel.QueueDeclare(
            queue: _options.DeadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        channel.QueueBind(
            queue: _options.DeadLetterQueueName,
            exchange: _options.DeadLetterExchangeName,
            routingKey: "#");

        var queueArguments = new Dictionary<string, object>
        {
            ["x-dead-letter-exchange"] = _options.DeadLetterExchangeName,
            ["x-dead-letter-routing-key"] = _options.DeadLetterQueueName
        };

        channel.QueueDeclare(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArguments);

        channel.QueueBind(
            queue: _options.QueueName,
            exchange: _options.ExchangeName,
            routingKey: _options.ReservaCreadaRoutingKey);

        _logger.LogInformation(
            "Topologia RabbitMQ declarada. Exchange={Exchange}, Queue={Queue}, DLQ={Dlq}",
            _options.ExchangeName,
            _options.QueueName,
            _options.DeadLetterQueueName);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}

