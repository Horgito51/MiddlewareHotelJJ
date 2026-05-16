using HotelJJ.DataAccess.Http.Models.Alojamiento.Requests;
using HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

namespace HotelJJ.DataAccess.Http.Interfaces;

public interface IAlojamientoHttpClient
{
    Task<PagedAlojamientoResponseModel<AlojamientoSearchItemResponseModel>> SearchAsync(
        SearchAlojamientoRequestModel request,
        CancellationToken cancellationToken = default);

    Task<AlojamientoDetailResponseModel> GetDetailAsync(
        Guid sucursalGuid,
        AlojamientoDetailRequestModel request,
        CancellationToken cancellationToken = default);

    Task<PagedAlojamientoResponseModel<AlojamientoReviewResponseModel>> GetReviewsAsync(
        Guid sucursalGuid,
        AlojamientoReviewsRequestModel request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlojamientoHabitacionResponseModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        AlojamientoHabitacionesRequestModel request,
        CancellationToken cancellationToken = default);
}
