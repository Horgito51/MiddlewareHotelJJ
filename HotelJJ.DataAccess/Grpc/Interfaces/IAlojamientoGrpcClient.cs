using HotelJJ.DataAccess.Http.Models.Alojamiento.Responses;

namespace HotelJJ.DataAccess.Grpc.Interfaces;

public interface IAlojamientoGrpcClient
{
    Task<AlojamientoDetailResponseModel> GetDetailAsync(
        Guid sucursalGuid,
        DateTime? fechaEntrada,
        DateTime? fechaSalida,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlojamientoHabitacionResponseModel>> GetHabitacionesAsync(
        Guid sucursalGuid,
        Guid? tipoHabitacionGuid,
        DateTime? fechaInicio,
        DateTime? fechaFin,
        CancellationToken cancellationToken = default);
}
