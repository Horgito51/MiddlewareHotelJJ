using HotelJJ.DataAccess.Http.Models.Facturacion.Requests;
using HotelJJ.DataAccess.Http.Models.Facturacion.Responses;

namespace HotelJJ.DataAccess.Grpc.Interfaces;

public interface IFacturacionGrpcClient
{
    Task<FacturaFacturacionResponseModel> GetByGuidAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaFacturacionResponseModel> GenerarFacturaReservaAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaFacturacionResponseModel> GenerarFacturaFinalAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoFacturacionResponseModel> RegistrarPagoAsync(
        PagoCreateFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoSimuladoFacturacionResponseModel> SimularPagoAsync(
        PagoSimularFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<decimal> GetSaldoAsync(
        int idFactura,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
