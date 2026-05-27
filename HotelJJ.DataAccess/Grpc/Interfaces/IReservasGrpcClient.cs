using HotelJJ.DataAccess.Http.Models.Reservas.Requests;
using HotelJJ.DataAccess.Http.Models.Reservas.Responses;

namespace HotelJJ.DataAccess.Grpc.Interfaces;

public interface IReservasGrpcClient
{
    Task<ReservaResponseModel> CreateAsync(
        CreateReservaRequestModel request,
        CancellationToken cancellationToken = default);

    Task<ReservaResponseModel> GetByGuidAsync(
        Guid reservaGuid,
        CancellationToken cancellationToken = default);

    Task<InternalReservaResponseModel> GetInternalForHospedajeAsync(
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
