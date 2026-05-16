using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataManagement.Hospedaje.Interfaces;
using HotelJJ.DataManagement.Hospedaje.Mappers;
using HotelJJ.DataManagement.Hospedaje.Models;

namespace HotelJJ.DataManagement.Hospedaje.Services;

public class HospedajeDataService : IHospedajeDataService
{
    private readonly IHospedajeHttpClient _hospedajeHttpClient;

    public HospedajeDataService(IHospedajeHttpClient hospedajeHttpClient)
    {
        _hospedajeHttpClient = hospedajeHttpClient;
    }

    public async Task<EstadiaDataModel> CheckInAsync(
        int idReservaHabitacion,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _hospedajeHttpClient.CheckInAsync(
            idReservaHabitacion,
            authorizationHeader,
            cancellationToken);

        var estadia = response.FirstOrDefault();
        if (estadia is null)
        {
            throw new DownstreamApiException(
                "Hospedaje",
                System.Net.HttpStatusCode.NoContent,
                "Hospedaje no devolvio la estadia creada en el check-in.");
        }

        return HospedajeDataMapper.ToDataModel(estadia);
    }

    public Task CheckOutAsync(
        int idEstadia,
        CheckOutHospedajeDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return _hospedajeHttpClient.CheckOutAsync(
            idEstadia,
            HospedajeDataMapper.ToHttpRequest(request),
            authorizationHeader,
            cancellationToken);
    }

    public async Task<EstadiaDataModel> GetByGuidAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var estadias = await GetAllAsync(authorizationHeader, cancellationToken);
        var estadia = estadias.FirstOrDefault(item => item.EstadiaGuid == estadiaGuid);
        if (estadia is null)
        {
            throw new DownstreamApiException(
                "Hospedaje",
                System.Net.HttpStatusCode.NotFound,
                "No se encontro la estadia solicitada en Hospedaje.");
        }

        return estadia;
    }

    public async Task<IReadOnlyList<EstadiaDataModel>> GetAllAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var estadias = await _hospedajeHttpClient.GetAllAsync(authorizationHeader, cancellationToken);
        return estadias.Select(HospedajeDataMapper.ToDataModel).ToList();
    }

    public async Task<CargoEstadiaDataModel> AddCargoAsync(
        int idEstadia,
        CargoHospedajeDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _hospedajeHttpClient.AddCargoAsync(
            idEstadia,
            HospedajeDataMapper.ToHttpRequest(request),
            authorizationHeader,
            cancellationToken);

        return HospedajeDataMapper.ToDataModel(response);
    }

    public async Task<CargoEstadiaDataModel> GetCargoByGuidAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var estadias = await GetAllAsync(authorizationHeader, cancellationToken);
        foreach (var estadia in estadias)
        {
            var cargos = await _hospedajeHttpClient.GetCargosByEstadiaAsync(
                estadia.IdEstadia,
                authorizationHeader,
                cancellationToken);

            var cargo = cargos.FirstOrDefault(item => item.CargoGuid == cargoGuid);
            if (cargo is not null)
            {
                return HospedajeDataMapper.ToDataModel(cargo);
            }
        }

        throw new DownstreamApiException(
            "Hospedaje",
            System.Net.HttpStatusCode.NotFound,
            "No se encontro el cargo solicitado en Hospedaje.");
    }

    public Task AnularCargoAsync(
        int idCargo,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        return _hospedajeHttpClient.AnularCargoAsync(idCargo, authorizationHeader, cancellationToken);
    }
}
