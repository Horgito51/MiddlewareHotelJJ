using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.DataManagement.Facturacion.Models;

namespace HotelJJ.Business.Mappers.Facturacion;

public static class FacturacionBusinessMapper
{
    public static FacturaDTO ToDTO(FacturaDataModel data)
    {
        return new FacturaDTO
        {
            FacturaGuid = data.FacturaGuid,
            IdReserva = data.IdReserva,
            NumeroFactura = data.NumeroFactura,
            TipoFactura = data.TipoFactura,
            FechaEmision = data.FechaEmision,
            Subtotal = data.Subtotal,
            ValorIva = data.ValorIva,
            DescuentoTotal = data.DescuentoTotal,
            Total = data.Total,
            SaldoPendiente = data.SaldoPendiente,
            Moneda = data.Moneda,
            ObservacionesFactura = data.ObservacionesFactura,
            Estado = data.Estado,
            FechaRegistroUtc = data.FechaRegistroUtc,
            Detalles = data.Detalles.Select(ToDTO).ToList()
        };
    }

    public static FacturaDetalleDTO ToDTO(FacturaDetalleDataModel data)
    {
        return new FacturaDetalleDTO
        {
            FacturaDetalleGuid = data.FacturaDetalleGuid,
            TipoItem = data.TipoItem,
            ReferenciaTipo = data.ReferenciaTipo,
            ReferenciaId = data.ReferenciaId,
            DescripcionItem = data.DescripcionItem,
            Cantidad = data.Cantidad,
            PrecioUnitario = data.PrecioUnitario,
            SubtotalLinea = data.SubtotalLinea,
            ValorIvaLinea = data.ValorIvaLinea,
            DescuentoLinea = data.DescuentoLinea,
            TotalLinea = data.TotalLinea,
            FechaRegistroUtc = data.FechaRegistroUtc
        };
    }

    public static PagoDTO ToDTO(PagoDataModel data)
    {
        return new PagoDTO
        {
            PagoGuid = data.PagoGuid,
            Monto = data.Monto,
            MetodoPago = data.MetodoPago,
            EsPagoElectronico = data.EsPagoElectronico,
            ProveedorPasarela = data.ProveedorPasarela,
            TransaccionExterna = data.TransaccionExterna,
            CodigoAutorizacion = data.CodigoAutorizacion,
            Referencia = data.Referencia,
            EstadoPago = data.EstadoPago,
            FechaPagoUtc = data.FechaPagoUtc,
            Moneda = data.Moneda,
            TipoCambio = data.TipoCambio,
            RespuestaPasarela = data.RespuestaPasarela,
            FechaRegistroUtc = data.FechaRegistroUtc
        };
    }

    public static PagoSimuladoDTO ToDTO(PagoSimuladoDataModel data)
    {
        return new PagoSimuladoDTO
        {
            CodigoReserva = data.CodigoReserva,
            Monto = data.Monto,
            EstadoPago = data.EstadoPago,
            EstadoReserva = data.EstadoReserva,
            TransaccionExterna = data.TransaccionExterna,
            CodigoAutorizacion = data.CodigoAutorizacion,
            Mensaje = data.Mensaje,
            FechaPagoUtc = data.FechaPagoUtc
        };
    }

    public static FacturaSaldoDTO ToDTO(FacturaSaldoDataModel data)
    {
        return new FacturaSaldoDTO
        {
            FacturaGuid = data.FacturaGuid,
            Total = data.Total,
            SaldoPendiente = data.SaldoPendiente,
            Moneda = data.Moneda,
            Estado = data.Estado
        };
    }

    public static PagoCreateDataRequest ToDataRequest(PagoCreateDTO dto, int idFactura, int idReserva)
    {
        return new PagoCreateDataRequest
        {
            IdFactura = idFactura,
            IdReserva = idReserva,
            Monto = dto.Monto,
            MetodoPago = dto.MetodoPago,
            EsPagoElectronico = dto.EsPagoElectronico,
            ProveedorPasarela = dto.ProveedorPasarela,
            TransaccionExterna = dto.TransaccionExterna,
            CodigoAutorizacion = dto.CodigoAutorizacion,
            Referencia = dto.Referencia,
            Moneda = dto.Moneda,
            TipoCambio = dto.TipoCambio
        };
    }

    public static PagoSimularDataRequest ToDataRequest(PagoSimularDTO dto, int idReserva)
    {
        return new PagoSimularDataRequest
        {
            IdReserva = idReserva,
            Monto = dto.Monto,
            TokenPago = dto.TokenPago,
            Referencia = dto.Referencia
        };
    }
}
