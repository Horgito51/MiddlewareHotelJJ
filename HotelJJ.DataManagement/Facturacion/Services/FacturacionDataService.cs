using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataManagement.Facturacion.Interfaces;
using HotelJJ.DataManagement.Facturacion.Mappers;
using HotelJJ.DataManagement.Facturacion.Models;

namespace HotelJJ.DataManagement.Facturacion.Services;

public class FacturacionDataService : IFacturacionDataService
{
    private readonly IFacturacionHttpClient _facturacionHttpClient;

    public FacturacionDataService(IFacturacionHttpClient facturacionHttpClient)
    {
        _facturacionHttpClient = facturacionHttpClient;
    }

    public async Task<FacturaDataModel> GetByGuidAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var facturas = await _facturacionHttpClient.GetFacturasAsync(authorizationHeader, cancellationToken);
        var factura = facturas.FirstOrDefault(item => item.GuidFactura == facturaGuid);
        if (factura is null)
        {
            throw new DownstreamApiException(
                "Facturacion",
                System.Net.HttpStatusCode.NotFound,
                "No se encontro la factura solicitada en Facturacion.");
        }

        return FacturacionDataMapper.ToDataModel(factura);
    }

    public async Task<FacturaDataModel> GenerarFacturaReservaAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _facturacionHttpClient.GenerarFacturaReservaAsync(
            idReserva,
            authorizationHeader,
            cancellationToken);

        return FacturacionDataMapper.ToDataModel(response);
    }

    public async Task<FacturaDataModel> GenerarFacturaFinalAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _facturacionHttpClient.GenerarFacturaFinalAsync(
            idReserva,
            authorizationHeader,
            cancellationToken);

        return FacturacionDataMapper.ToDataModel(response);
    }

    public async Task<PagoDataModel> RegistrarPagoAsync(
        PagoCreateDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _facturacionHttpClient.RegistrarPagoAsync(
            FacturacionDataMapper.ToHttpRequest(request),
            authorizationHeader,
            cancellationToken);

        return FacturacionDataMapper.ToDataModel(response);
    }

    public async Task<PagoSimuladoDataModel> SimularPagoAsync(
        PagoSimularDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var response = await _facturacionHttpClient.SimularPagoAsync(
            FacturacionDataMapper.ToHttpRequest(request),
            authorizationHeader,
            cancellationToken);

        return FacturacionDataMapper.ToDataModel(response);
    }

    public async Task<FacturaSaldoDataModel> GetSaldoAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var factura = await GetByGuidAsync(facturaGuid, authorizationHeader, cancellationToken);

        return new FacturaSaldoDataModel
        {
            FacturaGuid = factura.FacturaGuid,
            Total = factura.Total,
            SaldoPendiente = factura.SaldoPendiente,
            Moneda = factura.Moneda,
            Estado = factura.Estado
        };
    }
}
