using HotelJJ.DataManagement.Facturacion.Models;

namespace HotelJJ.DataManagement.Facturacion.Interfaces;

public interface IFacturacionDataService
{
    Task<FacturaDataModel> GetByGuidAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaDataModel> GenerarFacturaReservaAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaDataModel> GenerarFacturaFinalAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoDataModel> RegistrarPagoAsync(
        PagoCreateDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<PagoSimuladoDataModel> SimularPagoAsync(
        PagoSimularDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);

    Task<FacturaSaldoDataModel> GetSaldoAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default);
}
