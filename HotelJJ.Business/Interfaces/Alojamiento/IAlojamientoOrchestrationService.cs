using HotelJJ.Business.DTOs.Alojamiento;

namespace HotelJJ.Business.Interfaces.Alojamiento;

public interface IAlojamientoOrchestrationService
{
    Task<PagedAlojamientoDTO<AlojamientoSearchItemDTO>> SearchAsync(
        AlojamientoSearchDTO request,
        CancellationToken cancellationToken = default);

    Task<AlojamientoDetailDTO> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailQueryDTO request,
        CancellationToken cancellationToken = default);

    Task<PagedAlojamientoDTO<AlojamientoReviewDTO>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsQueryDTO request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlojamientoHabitacionDTO>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesQueryDTO request,
        CancellationToken cancellationToken = default);
}
