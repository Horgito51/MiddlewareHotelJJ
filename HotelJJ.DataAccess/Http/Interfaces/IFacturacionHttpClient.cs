using HotelJJ.DataAccess.Http.Models.Facturacion.Requests;
using HotelJJ.DataAccess.Http.Models.Facturacion.Responses;

namespace HotelJJ.DataAccess.Http.Interfaces;

public interface IFacturacionHttpClient
{
    Task<IReadOnlyList<FacturaFacturacionResponseModel>> GetFacturasAsync(
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaFacturacionResponseModel> GetFacturaByIdAsync(
        int idFactura,
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
}
