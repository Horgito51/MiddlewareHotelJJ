namespace HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;

public interface IEventBus
{
    Task PublishAsync<TEvent>(
        TEvent integrationEvent,
        string routingKey,
        CancellationToken cancellationToken = default)
        where TEvent : class;
}

