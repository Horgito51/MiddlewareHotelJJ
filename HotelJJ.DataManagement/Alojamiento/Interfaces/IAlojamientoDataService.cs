using HotelJJ.DataManagement.Alojamiento.Models;

namespace HotelJJ.DataManagement.Alojamiento.Interfaces;

public interface IAlojamientoDataService
{
    Task<PagedAlojamientoDataModel<AlojamientoSearchItemDataModel>> SearchAsync(
        AlojamientoSearchDataRequest request,
        CancellationToken cancellationToken = default);

    Task<AlojamientoDetailDataModel> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailDataRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedAlojamientoDataModel<AlojamientoReviewDataModel>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsDataRequest request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlojamientoHabitacionDataModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesDataRequest request,
        CancellationToken cancellationToken = default);
}
