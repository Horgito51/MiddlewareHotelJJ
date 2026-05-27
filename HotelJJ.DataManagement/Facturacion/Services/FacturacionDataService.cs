using HotelJJ.DataAccess.Http.Common;
using HotelJJ.DataAccess.Http.Interfaces;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataManagement.Facturacion.Interfaces;
using HotelJJ.DataManagement.Facturacion.Mappers;
using HotelJJ.DataManagement.Facturacion.Models;

namespace HotelJJ.DataManagement.Facturacion.Services;

public class FacturacionDataService : IFacturacionDataService
{
    private readonly IFacturacionHttpClient _facturacionHttpClient;
    private readonly IFacturacionGrpcClient _facturacionGrpcClient;

    public FacturacionDataService(
        IFacturacionHttpClient facturacionHttpClient,
        IFacturacionGrpcClient facturacionGrpcClient)
    {
        _facturacionHttpClient = facturacionHttpClient;
        _facturacionGrpcClient = facturacionGrpcClient;
    }

    public async Task<FacturaDataModel> GetByGuidAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _facturacionGrpcClient.GetByGuidAsync(
                facturaGuid,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException)
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
    }

    public async Task<FacturaDataModel> GenerarFacturaReservaAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _facturacionGrpcClient.GenerarFacturaReservaAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException)
        {
            var response = await _facturacionHttpClient.GenerarFacturaReservaAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
    }

    public async Task<FacturaDataModel> GenerarFacturaFinalAsync(
        int idReserva,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _facturacionGrpcClient.GenerarFacturaFinalAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException)
        {
            var response = await _facturacionHttpClient.GenerarFacturaFinalAsync(
                idReserva,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
    }

    public async Task<PagoDataModel> RegistrarPagoAsync(
        PagoCreateDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = FacturacionDataMapper.ToHttpRequest(request);
        try
        {
            var response = await _facturacionGrpcClient.RegistrarPagoAsync(
                httpRequest,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException)
        {
            var response = await _facturacionHttpClient.RegistrarPagoAsync(
                httpRequest,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
    }

    public async Task<PagoSimuladoDataModel> SimularPagoAsync(
        PagoSimularDataRequest request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = FacturacionDataMapper.ToHttpRequest(request);
        try
        {
            var response = await _facturacionGrpcClient.SimularPagoAsync(
                httpRequest,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
        catch (DownstreamApiException)
        {
            var response = await _facturacionHttpClient.SimularPagoAsync(
                httpRequest,
                authorizationHeader,
                cancellationToken);

            return FacturacionDataMapper.ToDataModel(response);
        }
    }

    public async Task<FacturaSaldoDataModel> GetSaldoAsync(
        Guid facturaGuid,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        var factura = await GetByGuidAsync(facturaGuid, authorizationHeader, cancellationToken);
        decimal saldoPendiente;
        try
        {
            saldoPendiente = await _facturacionGrpcClient.GetSaldoAsync(
                factura.IdFactura,
                authorizationHeader,
                cancellationToken);
        }
        catch (DownstreamApiException)
        {
            saldoPendiente = factura.SaldoPendiente;
        }

        return new FacturaSaldoDataModel
        {
            FacturaGuid = factura.FacturaGuid,
            Total = factura.Total,
            SaldoPendiente = saldoPendiente,
            Moneda = factura.Moneda,
            Estado = factura.Estado
        };
    }
}
