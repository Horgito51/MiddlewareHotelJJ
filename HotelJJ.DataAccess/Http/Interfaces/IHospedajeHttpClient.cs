using HotelJJ.DataAccess.Http.Models.Hospedaje.Requests;
using HotelJJ.DataAccess.Http.Models.Hospedaje.Responses;

namespace HotelJJ.DataAccess.Http.Interfaces;

public interface IHospedajeHttpClient
{
    Task<IReadOnlyList<EstadiaHospedajeResponseModel>> CheckInAsync(
        int idReservaHabitacion,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task CheckOutAsync(
        int idEstadia,
        CheckOutHospedajeRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<EstadiaHospedajeResponseModel> GetByIdAsync(
        int idEstadia,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EstadiaHospedajeResponseModel>> GetAllAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<CargoHospedajeResponseModel> AddCargoAsync(
        int idEstadia,
        CargoHospedajeRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CargoHospedajeResponseModel>> GetCargosByEstadiaAsync(
        int idEstadia,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task AnularCargoAsync(
        int idCargo,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
