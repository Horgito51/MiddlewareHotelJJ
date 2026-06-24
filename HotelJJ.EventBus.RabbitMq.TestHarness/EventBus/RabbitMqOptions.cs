namespace HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "hotel.integration.events";
    public string DeadLetterExchangeName { get; set; } = "hotel.integration.dlx";
    public string QueueName { get; set; } = "alojamiento.reservas.queue";
    public string DeadLetterQueueName { get; set; } = "alojamiento.reservas.dlq";
    public string ReservaCreadaRoutingKey { get; set; } = "reservas.reserva.creada.v1";
}

