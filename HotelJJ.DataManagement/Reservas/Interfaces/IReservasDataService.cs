using HotelJJ.DataManagement.Reservas.Models;
using HotelJJ.DataManagement.Hospedaje.Models;

namespace HotelJJ.DataManagement.Reservas.Interfaces;

public interface IReservasDataService
{
    Task<ReservaDataModel> CreateAsync(
        ReservaCreateDataRequest request,
        CancellationToken cancellationToken = default);

    Task<ReservaDataModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default);

    Task<ReservaHospedajeDataModel> GetInternalForHospedajeAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<ReservaPrecioDataModel> CalcularPrecioAsync(
        ReservaPrecioDataRequest request,
        CancellationToken cancellationToken = default);

    Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaDataRequest request,
        CancellationToken cancellationToken = default);
}
