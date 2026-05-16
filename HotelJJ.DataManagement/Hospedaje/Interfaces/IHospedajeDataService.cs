using HotelJJ.DataManagement.Hospedaje.Models;

namespace HotelJJ.DataManagement.Hospedaje.Interfaces;

public interface IHospedajeDataService
{
    Task<EstadiaDataModel> CheckInAsync(
        int idReservaHabitacion,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task CheckOutAsync(
        int idEstadia,
        CheckOutHospedajeDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<EstadiaDataModel> GetByGuidAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EstadiaDataModel>> GetAllAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<CargoEstadiaDataModel> AddCargoAsync(
        int idEstadia,
        CargoHospedajeDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<CargoEstadiaDataModel> GetCargoByGuidAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task AnularCargoAsync(
        int idCargo,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
