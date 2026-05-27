using HotelJJ.DataAccess.Http.Models.Reservas.Requests;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;

namespace HotelJJ.DataAccess.Http.Interfaces;

public interface IReservasHttpClient
{
    Task<ReservaResponseModel> CreateAsync(
        CreateReservaRequestModel request,
        CancellationToken cancellationToken = default);

    Task<ReservaResponseModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default);

    Task<ReservaResponseModel> GetByGuidAuthorizedAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<InternalReservaResponseModel> GetInternalByGuidAsync(
        Guid reservaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<ReservaPrecioResponseModel> CalcularPrecioAsync(
        ReservaPrecioRequestModel request,
        CancellationToken cancellationToken = default);

    Task CancelarAsync(
        Guid reservaGuid,
        CancelarReservaRequestModel request,
        CancellationToken cancellationToken = default);
}
