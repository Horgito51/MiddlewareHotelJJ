using System.Net;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Facturacion.Interfaces;
using HotelJJ.DataManagement.Hospedaje.Interfaces;
using HotelJJ.DataManagement.Reservas.Interfaces;

namespace HotelJJ.DataManagement.Common.Identifiers;

public class IdentifierResolverService : IIdentifierResolverService
{
    private readonly IReservasDataService _reservasDataService;
    private readonly IHospedajeDataService _hospedajeDataService;
    private readonly IFacturacionDataService _facturacionDataService;

    public IdentifierResolverService(
        IReservasDataService reservasDataService,
        IHospedajeDataService hospedajeDataService,
        IFacturacionDataService facturacionDataService)
    {
        _reservasDataService = reservasDataService;
        _hospedajeDataService = hospedajeDataService;
        _facturacionDataService = facturacionDataService;
    }

    public async Task<int> ResolveReservaIdAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var reserva = await _reservasDataService.GetInternalForHospedajeAsync(
            reservaGuid,
            authorizationHeader,
            cancellationToken);

        ValidateIdentifier(reserva.IdReserva, nameof(reserva.IdReserva), "MID-IDR-RES");
        return reserva.IdReserva;
    }

    public async Task<int> ResolveReservaHabitacionIdAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var reserva = await _reservasDataService.GetInternalForHospedajeAsync(
            reservaGuid,
            authorizationHeader,
            cancellationToken);

        var habitacion = reserva.Habitaciones.FirstOrDefault();
        if (habitacion is null)
        {
            throw new DownstreamApiException(
                "IdentifierResolver",
                HttpStatusCode.BadRequest,
                "No existe detalle de habitacion para la reserva especificada.");
        }

        ValidateIdentifier(habitacion.IdReservaHabitacion, nameof(habitacion.IdReservaHabitacion), "MID-IDR-RES-HAB");
        return habitacion.IdReservaHabitacion;
    }

    public async Task<int> ResolveEstadiaIdAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var estadia = await _hospedajeDataService.GetByGuidAsync(
            estadiaGuid,
            authorizationHeader,
            cancellationToken);

        ValidateIdentifier(estadia.IdEstadia, nameof(estadia.IdEstadia), "MID-IDR-EST");
        return estadia.IdEstadia;
    }

    public async Task<int> ResolveCargoIdAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var cargo = await _hospedajeDataService.GetCargoByGuidAsync(
            cargoGuid,
            authorizationHeader,
            cancellationToken);

        ValidateIdentifier(cargo.IdCargoEstadia, nameof(cargo.IdCargoEstadia), "MID-IDR-CAR");
        return cargo.IdCargoEstadia;
    }

    public async Task<int> ResolveFacturaIdAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var factura = await _facturacionDataService.GetByGuidAsync(
            facturaGuid,
            authorizationHeader,
            cancellationToken);

        ValidateIdentifier(factura.IdFactura, nameof(factura.IdFactura), "MID-IDR-FAC");
        return factura.IdFactura;
    }

    private static void ValidateIdentifier(int value, string field, string code)
    {
        if (value <= 0)
        {
            throw new DownstreamApiException(
                "IdentifierResolver",
                HttpStatusCode.BadRequest,
                $"No se pudo resolver un identificador interno valido para '{field}'.");
        }
    }
}
