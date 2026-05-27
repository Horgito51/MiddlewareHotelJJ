using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataManagement.Alojamiento.Interfaces;
using HotelJJ.DataManagement.Alojamiento.Mappers;
using HotelJJ.DataManagement.Alojamiento.Models;

namespace HotelJJ.DataManagement.Alojamiento.Services;

public class AlojamientoDataService : IAlojamientoDataService
{
    private readonly IAlojamientoHttpClient _alojamientoHttpClient;
    private readonly IAlojamientoGrpcClient _alojamientoGrpcClient;

    public AlojamientoDataService(
        IAlojamientoHttpClient alojamientoHttpClient,
        IAlojamientoGrpcClient alojamientoGrpcClient)
    {
        _alojamientoHttpClient = alojamientoHttpClient;
        _alojamientoGrpcClient = alojamientoGrpcClient;
    }

    public async Task<PagedAlojamientoDataModel<AlojamientoSearchItemDataModel>> SearchAsync(
        AlojamientoSearchDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _alojamientoHttpClient.SearchAsync(
            AlojamientoDataMapper.ToHttpRequest(request),
            cancellationToken);

        return AlojamientoDataMapper.ToPagedDataModel(response, AlojamientoDataMapper.ToDataModel);
    }

    public async Task<AlojamientoDetailDataModel> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _alojamientoHttpClient.GetDetailAsync(
            sucursalGuid,
            AlojamientoDataMapper.ToHttpRequest(request),
            cancellationToken);

        return AlojamientoDataMapper.ToDataModel(response);
    }

    public async Task<PagedAlojamientoDataModel<AlojamientoReviewDataModel>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsDataRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _alojamientoHttpClient.GetReviewsAsync(
            sucursalGuid,
            AlojamientoDataMapper.ToHttpRequest(request),
            cancellationToken);

        return AlojamientoDataMapper.ToPagedDataModel(response, AlojamientoDataMapper.ToDataModel);
    }

    public async Task<IReadOnlyList<AlojamientoHabitacionDataModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesDataRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _alojamientoGrpcClient.GetHabitacionesAsync(
                sucursalGuid,
                request.TipoHabitacionGuid,
                request.FechaInicio,
                request.FechaFin,
                cancellationToken);

            return response.Select(AlojamientoDataMapper.ToDataModel).ToList();
        }
        catch (DownstreamApiException)
        {
            var response = await _alojamientoHttpClient.GetHabitacionesAsync(
                sucursalGuid,
                AlojamientoDataMapper.ToHttpRequest(request),
                cancellationToken);

            return response.Select(AlojamientoDataMapper.ToDataModel).ToList();
        }
    }
}
