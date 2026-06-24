using HotelJJ.EventBus.RabbitMq.TestHarness.EventBus;
using HotelJJ.EventBus.RabbitMq.TestHarness.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var mode = args.FirstOrDefault()?.Trim().ToLowerInvariant() ?? "both";
var validModes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    "publish",
    "consume",
    "both"
};

if (!validModes.Contains(mode))
{
    Console.WriteLine("Modo no soportado. Usa: publish, consume o both.");
    return 2;
}

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.Configure<RabbitMqOptions>(options =>
{
    options.HostName = "localhost";
    options.Port = 5672;
    options.UserName = "guest";
    options.Password = "guest";
    options.VirtualHost = "/";
    options.ExchangeName = "hotel.integration.events";
    options.DeadLetterExchangeName = "hotel.integration.dlx";
    options.QueueName = "alojamiento.reservas.queue";
    options.DeadLetterQueueName = "alojamiento.reservas.dlq";
    options.ReservaCreadaRoutingKey = "reservas.reserva.creada.v1";
});

builder.Services.AddSingleton<RabbitMqConnection>();
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
builder.Services.AddSingleton<ReservaCreadaTestPublisher>();

if (mode is "consume" or "both")
{
    builder.Services.AddHostedService<AlojamientoReservasTestConsumer>();
}

if (mode == "both")
{
    builder.Services.AddHostedService<PublishOnceHostedService>();
}

using var host = builder.Build();

if (mode == "publish")
{
    var publisher = host.Services.GetRequiredService<ReservaCreadaTestPublisher>();
    await publisher.PublishAsync();
    return 0;
}

await host.RunAsync();
return 0;

internal sealed class PublishOnceHostedService : BackgroundService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ILogger<PublishOnceHostedService> _logger;
    private readonly ReservaCreadaTestPublisher _publisher;

    public PublishOnceHostedService(
        ReservaCreadaTestPublisher publisher,
        IHostApplicationLifetime applicationLifetime,
        ILogger<PublishOnceHostedService> logger)
    {
        _publisher = publisher;
        _applicationLifetime = applicationLifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        await _publisher.PublishAsync(stoppingToken);
        _logger.LogInformation("Modo both: evento de prueba publicado; el consumer queda activo.");

        _applicationLifetime.ApplicationStopping.Register(() =>
            _logger.LogInformation("Deteniendo test harness RabbitMQ."));
    }
}

