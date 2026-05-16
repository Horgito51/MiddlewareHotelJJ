using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataManagement.Reservas.Interfaces;
using HotelJJ.DataManagement.Reservas.Mappers;
using HotelJJ.DataManagement.Reservas.Models;
using HotelJJ.DataManagement.Hospedaje.Models;
using HotelJJ.DataManagement.Hospedaje.Mappers;

namespace HotelJJ.DataManagement.Reservas.Services;

public class ReservasDataService : IReservasDataService
{
    private readonly IReservasHttpClient _reservasHttpClient;

    public ReservasDataService(IReservasHttpClient reservasHttpClient)
    {
        _reservasHttpClient = reservasHttpClient;
    }

    public async Task<ReservaDataModel> CreateAsync(
        ReservaCreateDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _reservasHttpClient.CreateAsync(
            ReservasDataMapper.ToHttpRequest(request),
            cancellationToken);

        return ReservasDataMapper.ToDataModel(response);
    }

    public async Task<ReservaDataModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default)
    {
        var response = await _reservasHttpClient.GetByGuidAsync(reservaGuid, cancellationToken);
        return ReservasDataMapper.ToDataModel(response);
    }

    public async Task<ReservaHospedajeDataModel> GetInternalForHospedajeAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _reservasHttpClient.GetInternalByGuidAsync(
            reservaGuid,
            authorizationHeader,
            cancellationToken);

        return HospedajeDataMapper.ToDataModel(response);
    }

    public async Task<ReservaPrecioDataModel> CalcularPrecioAsync(
        ReservaPrecioDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _reservasHttpClient.CalcularPrecioAsync(
            ReservasDataMapper.ToHttpRequest(request),
            cancellationToken);

        return ReservasDataMapper.ToDataModel(response);
    }

    public Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaDataRequest request,
        CancellationToken cancellationToken = default)
    {
        return _reservasHttpClient.CancelarAsync(
            reservaGuid,
            ReservasDataMapper.ToHttpRequest(request),
            cancellationToken);
    }
}
