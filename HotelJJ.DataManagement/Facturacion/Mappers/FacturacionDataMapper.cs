using HotelJJ.DataAccess.Http.Models.Facturacion.Requests;
using HotelJJ.DataAccess.Http.Models.Facturacion.Responses;
using HotelJJ.DataManagement.Facturacion.Models;

namespace HotelJJ.DataManagement.Facturacion.Mappers;

public static class FacturacionDataMapper
{
    public static FacturaDataModel ToDataModel(FacturaFacturacionResponseModel response)
    {
        return new FacturaDataModel
        {
            IdFactura = response.IdFactura,
            FacturaGuid = response.GuidFactura,
            IdCliente = response.IdCliente,
            IdReserva = response.IdReserva,
            IdSucursal = response.IdSucursal,
            NumeroFactura = response.NumeroFactura,
            TipoFactura = response.TipoFactura,
            FechaEmision = response.FechaEmision,
            Subtotal = response.Subtotal,
            ValorIva = response.ValorIva,
            DescuentoTotal = response.DescuentoTotal,
            Total = response.Total,
            SaldoPendiente = response.SaldoPendiente,
            Moneda = response.Moneda,
            ObservacionesFactura = response.ObservacionesFactura,
            OrigenCanalFactura = response.OrigenCanalFactura,
            Estado = response.Estado,
            FechaRegistroUtc = response.FechaRegistroUtc,
            Detalles = response.Detalles?.Select(ToDataModel).ToList() ?? []
        };
    }

    public static FacturaDetalleDataModel ToDataModel(FacturaDetalleFacturacionResponseModel response)
    {
        return new FacturaDetalleDataModel
        {
            IdFacturaDetalle = response.IdFacturaDetalle,
            FacturaDetalleGuid = response.FacturaDetalleGuid,
            TipoItem = response.TipoItem,
            ReferenciaTipo = response.ReferenciaTipo,
            ReferenciaId = response.ReferenciaId,
            DescripcionItem = response.DescripcionItem,
            Cantidad = response.Cantidad,
            PrecioUnitario = response.PrecioUnitario,
            SubtotalLinea = response.SubtotalLinea,
            ValorIvaLinea = response.ValorIvaLinea,
            DescuentoLinea = response.DescuentoLinea,
            TotalLinea = response.TotalLinea,
            FechaRegistroUtc = response.FechaRegistroUtc
        };
    }

    public static PagoDataModel ToDataModel(PagoFacturacionResponseModel response)
    {
        return new PagoDataModel
        {
            IdPago = response.IdPago,
            PagoGuid = response.PagoGuid,
            IdFactura = response.IdFactura,
            IdReserva = response.IdReserva,
            Monto = response.Monto,
            MetodoPago = response.MetodoPago,
            EsPagoElectronico = response.EsPagoElectronico,
            ProveedorPasarela = response.ProveedorPasarela,
            TransaccionExterna = response.TransaccionExterna,
            CodigoAutorizacion = response.CodigoAutorizacion,
            Referencia = response.Referencia,
            EstadoPago = response.EstadoPago,
            FechaPagoUtc = response.FechaPagoUtc,
            Moneda = response.Moneda,
            TipoCambio = response.TipoCambio,
            RespuestaPasarela = response.RespuestaPasarela,
            FechaRegistroUtc = response.FechaRegistroUtc
        };
    }

    public static PagoSimuladoDataModel ToDataModel(PagoSimuladoFacturacionResponseModel response)
    {
        return new PagoSimuladoDataModel
        {
            IdReserva = response.IdReserva,
            CodigoReserva = response.CodigoReserva,
            Monto = response.Monto,
            EstadoPago = response.EstadoPago,
            EstadoReserva = response.EstadoReserva,
            TransaccionExterna = response.TransaccionExterna,
            CodigoAutorizacion = response.CodigoAutorizacion,
            Mensaje = response.Mensaje,
            FechaPagoUtc = response.FechaPagoUtc
        };
    }

    public static PagoCreateFacturacionRequestModel ToHttpRequest(PagoCreateDataRequest request)
    {
        return new PagoCreateFacturacionRequestModel
        {
            IdFactura = request.IdFactura,
            IdReserva = request.IdReserva,
            Monto = request.Monto,
            MetodoPago = request.MetodoPago,
            EsPagoElectronico = request.EsPagoElectronico,
            ProveedorPasarela = request.ProveedorPasarela,
            TransaccionExterna = request.TransaccionExterna,
            CodigoAutorizacion = request.CodigoAutorizacion,
            Referencia = request.Referencia,
            Moneda = request.Moneda,
            TipoCambio = request.TipoCambio
        };
    }

    public static PagoSimularFacturacionRequestModel ToHttpRequest(PagoSimularDataRequest request)
    {
        return new PagoSimularFacturacionRequestModel
        {
            IdReserva = request.IdReserva,
            Monto = request.Monto,
            TokenPago = request.TokenPago,
            Referencia = request.Referencia
        };
    }
}
