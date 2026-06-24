using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;

namespace HotelJJ.EventBus.RabbitMq.TestHarness.Services;

public sealed class AlojamientoReservasTestConsumer : BackgroundService
{
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<AlojamientoReservasTestConsumer> _logger;
    private readonly RabbitMqOptions _options;
    private IModel? _channel;

    public AlojamientoReservasTestConsumer(
        RabbitMqConnection connection,
        IOptions<RabbitMqOptions> options,
        ILogger<AlojamientoReservasTestConsumer> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connection.CreateChannel();
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 10, global: false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (_, args) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var correlationId = args.BasicProperties?.CorrelationId;
                var messageId = args.BasicProperties?.MessageId;

                _logger.LogInformation(
                    "Evento recibido en {Queue}. MessageId={MessageId}, CorrelationId={CorrelationId}, RoutingKey={RoutingKey}, Body={Body}",
                    _options.QueueName,
                    messageId,
                    correlationId,
                    args.RoutingKey,
                    body);

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando evento. Se enviara a DLQ si RabbitMQ aplica dead-letter.");
                _channel?.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
            }
        };

        _channel.BasicConsume(
            queue: _options.QueueName,
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation(
            "Consumer activo escuchando queue {Queue}. Presiona Ctrl+C para detener.",
            _options.QueueName);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}

