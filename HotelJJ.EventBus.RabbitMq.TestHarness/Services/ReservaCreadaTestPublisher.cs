using HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;
using HotelJJ.EventBus.RabbitMq.TestHarness.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelJJ.EventBus.RabbitMq.TestHarness.Services;

public sealed class ReservaCreadaTestPublisher
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<ReservaCreadaTestPublisher> _logger;
    private readonly RabbitMqOptions _options;

    public ReservaCreadaTestPublisher(
        IEventBus eventBus,
        IOptions<RabbitMqOptions> options,
        ILogger<ReservaCreadaTestPublisher> logger)
    {
        _eventBus = eventBus;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync(CancellationToken cancellationToken = default)
    {
        var correlationId = Guid.NewGuid();
        var testEvent = new ReservaCreadaTest
        {
            ReservaGuid = Guid.NewGuid(),
            SucursalGuid = Guid.NewGuid(),
            ClienteGuid = Guid.NewGuid(),
            CorrelationId = correlationId
        };

        await _eventBus.PublishAsync(
            testEvent,
            _options.ReservaCreadaRoutingKey,
            cancellationToken);

        _logger.LogInformation(
            "ReservaCreadaTest enviada. reservaGuid={ReservaGuid}, sucursalGuid={SucursalGuid}, clienteGuid={ClienteGuid}, correlationId={CorrelationId}",
            testEvent.ReservaGuid,
            testEvent.SucursalGuid,
            testEvent.ClienteGuid,
            testEvent.CorrelationId);
    }
}

