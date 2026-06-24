namespace HotelJJ.EventBus.RabbitMq.TestHarness.Events;

public sealed record ReservaCreadaTest
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = "reservas.reserva.creada.test";
    public string EventVersion { get; init; } = "v1";
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public Guid ReservaGuid { get; init; }
    public Guid SucursalGuid { get; init; }
    public Guid ClienteGuid { get; init; }
    public Guid CorrelationId { get; init; }
    public string Source { get; init; } = "hoteljj-eventbus-test-harness";
}

