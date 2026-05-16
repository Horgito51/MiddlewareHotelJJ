using HotelJJ.API.Models.Settings;
using Microsoft.Extensions.Options;

namespace HotelJJ.API.Infrastructure.Proxy;

public class ProxyRouteResolver
{
    private readonly MicroserviciosSettings _settings;

    public ProxyRouteResolver(IOptions<MicroserviciosSettings> settings)
    {
        _settings = settings.Value;
    }

    public Uri ResolveBaseUri(string microserviceName)
    {
        var baseUrl = microserviceName.ToUpperInvariant() switch
        {
            "SEGURIDAD" => _settings.Seguridad.BaseUrl,
            "ALOJAMIENTO" => _settings.Alojamiento.BaseUrl,
            "RESERVAS" => _settings.Reservas.BaseUrl,
            "HOSPEDAJE" => _settings.Hospedaje.BaseUrl,
            "FACTURACION" => _settings.Facturacion.BaseUrl,
            _ => throw new InvalidOperationException($"Microservicio desconocido: {microserviceName}.")
        };

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException($"No hay BaseUrl configurada para {microserviceName}.");
        }

        return new Uri(baseUrl, UriKind.Absolute);
    }
}
