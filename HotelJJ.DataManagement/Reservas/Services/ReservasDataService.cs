using System.Net;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Reservas.Interfaces;
using HotelJJ.DataManagement.Reservas.Mappers;
using HotelJJ.DataManagement.Reservas.Models;
using HotelJJ.DataManagement.Hospedaje.Models;
using HotelJJ.DataManagement.Hospedaje.Mappers;

namespace HotelJJ.DataManagement.Reservas.Services;

public class ReservasDataService : IReservasDataService
{
    private readonly IReservasHttpClient _reservasHttpClient;
    private readonly IReservasGrpcClient _reservasGrpcClient;

    public ReservasDataService(
        IReservasHttpClient reservasHttpClient,
        IReservasGrpcClient reservasGrpcClient)
    {
        _reservasHttpClient = reservasHttpClient;
        _reservasGrpcClient = reservasGrpcClient;
    }

    public async Task<ReservaDataModel> CreateAsync(
        ReservaCreateDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = ReservasDataMapper.ToHttpRequest(request);
        try
        {
            var response = await _reservasGrpcClient.CreateAsync(httpRequest, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException ex) when (ShouldFallbackToHttp(ex))
        {
            var response = await _reservasHttpClient.CreateAsync(httpRequest, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
    }

    public async Task<ReservaDataModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _reservasGrpcClient.GetByGuidAsync(reservaGuid, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException ex) when (ShouldFallbackToHttp(ex))
        {
            var response = await _reservasHttpClient.GetByGuidAsync(reservaGuid, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
    }

    public async Task<ReservaDataModel> GetByGuidAuthorizedAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _reservasHttpClient.GetByGuidAuthorizedAsync(
            reservaGuid,
            authorizationHeader,
            cancellationToken);

        return ReservasDataMapper.ToDataModel(response);
    }

    public async Task<ReservaHospedajeDataModel> GetInternalForHospedajeAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _reservasGrpcClient.GetInternalForHospedajeAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            return HospedajeDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException ex) when (ShouldFallbackToHttp(ex))
        {
            var response = await _reservasHttpClient.GetInternalByGuidAsync(
                reservaGuid,
                authorizationHeader,
                cancellationToken);

            return HospedajeDataMapper.ToDataModel(response);
        }
    }

    public async Task<ReservaPrecioDataModel> CalcularPrecioAsync(
        ReservaPrecioDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = ReservasDataMapper.ToHttpRequest(request);
        try
        {
            var response = await _reservasGrpcClient.CalcularPrecioAsync(httpRequest, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException ex) when (ShouldFallbackToHttp(ex))
        {
            var response = await _reservasHttpClient.CalcularPrecioAsync(httpRequest, cancellationToken);
            return ReservasDataMapper.ToDataModel(response);
        }
    }

    public async Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = ReservasDataMapper.ToHttpRequest(request);
        try
        {
            await _reservasGrpcClient.CancelarAsync(
                reservaGuid,
                httpRequest,
                cancellationToken);
        }
        catch (DownstreamApiException ex) when (ShouldFallbackToHttp(ex))
        {
            await _reservasHttpClient.CancelarAsync(
                reservaGuid,
                httpRequest,
                cancellationToken);
        }
    }

    private static bool ShouldFallbackToHttp(DownstreamApiException ex)
    {
        return ex.StatusCode is HttpStatusCode.BadGateway
            or HttpStatusCode.ServiceUnavailable
            or HttpStatusCode.GatewayTimeout;
    }
}
