using HotelJJ.Business.DTOs.Reservas;

namespace HotelJJ.Business.Interfaces.Reservas;

public interface IReservationOrchestrationService
{
    Task<ReservationDTO> CreateAsync(
        ReservationCreateDTO request,
        CancellationToken cancellationToken = default);

    Task<ReservationDTO> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default);

    Task<ReservationDTO> GetByGuidAuthorizedAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<ReservationPriceDTO> CalculatePriceAsync(
        ReservationPriceRequestDTO request,
        CancellationToken cancellationToken = default);

    Task CancelAsync(
        Guid reservaGuid,
        CancelReservationDTO request,
        CancellationToken cancellationToken = default);
}
