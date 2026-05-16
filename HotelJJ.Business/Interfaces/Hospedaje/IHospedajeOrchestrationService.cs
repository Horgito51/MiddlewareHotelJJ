using HotelJJ.Business.DTOs.Hospedaje;

namespace HotelJJ.Business.Interfaces.Hospedaje;

public interface IHospedajeOrchestrationService
{
    Task<EstadiaDTO> CheckInAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<EstadiaDTO> GetByGuidAsync(
        Guid estadiaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task CheckOutAsync(
        Guid estadiaGuid,
        CheckOutHospedajeDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<CargoEstadiaDTO> AddCargoAsync(
        Guid estadiaGuid,
        CargoHospedajeCreateDTO request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task AnularCargoAsync(
        Guid cargoGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
