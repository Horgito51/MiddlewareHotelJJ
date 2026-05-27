using Facturacion.Contracts.Grpc.V1;
using Grpc.Core;
using HotelJJ.DataAccess.Grpc.Common;
using HotelJJ.DataAccess.Grpc.Interfaces;
using HotelJJ.DataAccess.Http.Models.Facturacion.Requests;
using HotelJJ.DataAccess.Http.Models.Facturacion.Responses;

namespace HotelJJ.DataAccess.Grpc.Clients;

public class FacturacionGrpcClient : IFacturacionGrpcClient
{
    private readonly Facturacion.Contracts.Grpc.V1.FacturacionGrpc.FacturacionGrpcClient _client;
    private readonly PagoGrpc.PagoGrpcClient _pagoClient;

    public FacturacionGrpcClient(
        Facturacion.Contracts.Grpc.V1.FacturacionGrpc.FacturacionGrpcClient client,
        PagoGrpc.PagoGrpcClient pagoClient)
    {
        _client = client;
        _pagoClient = pagoClient;
    }

    public async Task<FacturaFacturacionResponseModel> GetByGuidAsync(Guid facturaGuid, string? authorizationHeader, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetFacturaByGuidAsync(new GuidRequest { Guid = facturaGuid.ToString() }, cancellationToken: cancellationToken);
            return ToData(response);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    public async Task<FacturaFacturacionResponseModel> GenerarFacturaReservaAsync(int idReserva, string? authorizationHeader, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GenerarFacturaReservaAsync(new GenerarFacturaRequest
            {
                IdReserva = idReserva,
                Usuario = "Middleware.HotelJJ"
            }, cancellationToken: cancellationToken);

            var factura = await _client.GetFacturaByIdAsync(new IdRequest { Id = response.IdFactura }, cancellationToken: cancellationToken);
            return ToData(factura);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    public async Task<FacturaFacturacionResponseModel> GenerarFacturaFinalAsync(int idReserva, string? authorizationHeader, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GenerarFacturaFinalAsync(new GenerarFacturaRequest
            {
                IdReserva = idReserva,
                Usuario = "Middleware.HotelJJ"
            }, cancellationToken: cancellationToken);

            var factura = await _client.GetFacturaByIdAsync(new IdRequest { Id = response.IdFactura }, cancellationToken: cancellationToken);
            return ToData(factura);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    public async Task<PagoFacturacionResponseModel> RegistrarPagoAsync(
        PagoCreateFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _pagoClient.CreatePagoAsync(new PagoCreateRequest
            {
                IdFactura = request.IdFactura,
                IdReserva = request.IdReserva,
                Monto = FormatDecimal(request.Monto),
                MetodoPago = request.MetodoPago,
                EsPagoElectronico = request.EsPagoElectronico,
                ProveedorPasarela = request.ProveedorPasarela ?? string.Empty,
                TransaccionExterna = request.TransaccionExterna ?? string.Empty,
                CodigoAutorizacion = request.CodigoAutorizacion ?? string.Empty,
                Referencia = request.Referencia ?? string.Empty,
                Moneda = request.Moneda,
                TipoCambio = FormatDecimal(request.TipoCambio)
            }, cancellationToken: cancellationToken);

            return ToData(response);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    public async Task<PagoSimuladoFacturacionResponseModel> SimularPagoAsync(
        PagoSimularFacturacionRequestModel request,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _pagoClient.SimularPagoAsync(new PagoSimularRequest
            {
                IdReserva = request.IdReserva,
                Monto = FormatDecimal(request.Monto),
                TokenPago = request.TokenPago,
                Referencia = request.Referencia ?? string.Empty,
                Usuario = "Middleware.HotelJJ"
            }, cancellationToken: cancellationToken);

            return new PagoSimuladoFacturacionResponseModel
            {
                IdReserva = response.IdReserva,
                CodigoReserva = response.CodigoReserva,
                Monto = ParseDecimal(response.Monto),
                EstadoPago = response.EstadoPago,
                EstadoReserva = response.EstadoReserva,
                TransaccionExterna = response.TransaccionExterna,
                CodigoAutorizacion = response.CodigoAutorizacion,
                Mensaje = response.Mensaje,
                FechaPagoUtc = response.FechaPagoUtc.ToDateTime()
            };
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    public async Task<decimal> GetSaldoAsync(
        int idFactura,
        string? authorizationHeader,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetSaldoPendienteAsync(
                new IdRequest { Id = idFactura },
                cancellationToken: cancellationToken);
            return ParseDecimal(response.SaldoPendiente);
        }
        catch (RpcException ex)
        {
            throw GrpcStatusMapper.ToDownstream("Facturacion", ex);
        }
    }

    private static FacturaFacturacionResponseModel ToData(Factura response)
    {
        return new FacturaFacturacionResponseModel
        {
            IdFactura = response.IdFactura,
            GuidFactura = Guid.TryParse(response.FacturaGuid, out var guid) ? guid : Guid.Empty,
            IdCliente = response.IdCliente,
            IdReserva = response.IdReserva,
            IdSucursal = response.IdSucursal,
            NumeroFactura = response.NumeroFactura,
            TipoFactura = response.TipoFactura,
            FechaEmision = response.FechaEmision.ToDateTime(),
            Subtotal = ParseDecimal(response.Subtotal),
            ValorIva = ParseDecimal(response.ValorIva),
            DescuentoTotal = ParseDecimal(response.DescuentoTotal),
            Total = ParseDecimal(response.Total),
            SaldoPendiente = ParseDecimal(response.SaldoPendiente),
            Moneda = response.Moneda,
            ObservacionesFactura = response.ObservacionesFactura,
            OrigenCanalFactura = response.OrigenCanalFactura,
            Estado = response.Estado,
            Detalles = response.Detalles.Select(d => new FacturaDetalleFacturacionResponseModel
            {
                IdFacturaDetalle = d.IdFacturaDetalle,
                FacturaDetalleGuid = Guid.TryParse(d.FacturaDetalleGuid, out var detalleGuid) ? detalleGuid : Guid.Empty,
                TipoItem = d.TipoItem,
                ReferenciaTipo = d.ReferenciaTipo,
                ReferenciaId = d.ReferenciaId,
                DescripcionItem = d.DescripcionItem,
                Cantidad = d.Cantidad,
                PrecioUnitario = ParseDecimal(d.PrecioUnitario),
                SubtotalLinea = ParseDecimal(d.SubtotalLinea),
                ValorIvaLinea = ParseDecimal(d.ValorIvaLinea),
                DescuentoLinea = ParseDecimal(d.DescuentoLinea),
                TotalLinea = ParseDecimal(d.TotalLinea)
            }).ToList()
        };
    }

    private static PagoFacturacionResponseModel ToData(Pago response)
    {
        return new PagoFacturacionResponseModel
        {
            IdPago = response.IdPago,
            PagoGuid = Guid.TryParse(response.PagoGuid, out var pagoGuid) ? pagoGuid : Guid.Empty,
            IdFactura = response.IdFactura,
            IdReserva = response.IdReserva,
            Monto = ParseDecimal(response.Monto),
            MetodoPago = response.MetodoPago,
            EsPagoElectronico = response.EsPagoElectronico,
            ProveedorPasarela = response.ProveedorPasarela,
            TransaccionExterna = response.TransaccionExterna,
            CodigoAutorizacion = response.CodigoAutorizacion,
            Referencia = response.Referencia,
            EstadoPago = response.EstadoPago,
            FechaPagoUtc = response.FechaPagoUtc.ToDateTime(),
            Moneda = response.Moneda,
            TipoCambio = ParseDecimal(response.TipoCambio),
            RespuestaPasarela = response.RespuestaPasarela
        };
    }

    private static string FormatDecimal(decimal value)
    {
        return value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
    }

    private static decimal ParseDecimal(string value)
    {
        return decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : 0m;
    }
}
