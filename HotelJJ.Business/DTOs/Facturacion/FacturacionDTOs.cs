namespace HotelJJ.Business.DTOs.Facturacion;

public class FacturaDTO
{
    public Guid FacturaGuid { get; set; }
    public int IdReserva { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public string TipoFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ValorIva { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Total { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Moneda { get; set; } = string.Empty;
    public string? ObservacionesFactura { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaRegistroUtc { get; set; }
    public IReadOnlyList<FacturaDetalleDTO> Detalles { get; set; } = Array.Empty<FacturaDetalleDTO>();
}

public class FacturaDetalleDTO
{
    public Guid FacturaDetalleGuid { get; set; }
    public string TipoItem { get; set; } = string.Empty;
    public string? ReferenciaTipo { get; set; }
    public int? ReferenciaId { get; set; }
    public string DescripcionItem { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal SubtotalLinea { get; set; }
    public decimal ValorIvaLinea { get; set; }
    public decimal DescuentoLinea { get; set; }
    public decimal TotalLinea { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
}

public class PagoDTO
{
    public Guid PagoGuid { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public bool EsPagoElectronico { get; set; }
    public string? ProveedorPasarela { get; set; }
    public string? TransaccionExterna { get; set; }
    public string? CodigoAutorizacion { get; set; }
    public string? Referencia { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public DateTime FechaPagoUtc { get; set; }
    public string Moneda { get; set; } = string.Empty;
    public decimal TipoCambio { get; set; }
    public string? RespuestaPasarela { get; set; }
    public DateTime FechaRegistroUtc { get; set; }
}

public class PagoCreateDTO
{
    public Guid FacturaGuid { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public bool EsPagoElectronico { get; set; }
    public string ProveedorPasarela { get; set; } = string.Empty;
    public string? TransaccionExterna { get; set; }
    public string? CodigoAutorizacion { get; set; }
    public string? Referencia { get; set; }
    public string Moneda { get; set; } = "USD";
    public decimal TipoCambio { get; set; } = 1;
}

public class PagoSimularDTO
{
    public Guid ReservaGuid { get; set; }
    public decimal Monto { get; set; }
    public string TokenPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class PagoSimuladoDTO
{
    public string CodigoReserva { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string EstadoPago { get; set; } = string.Empty;
    public string EstadoReserva { get; set; } = string.Empty;
    public string TransaccionExterna { get; set; } = string.Empty;
    public string CodigoAutorizacion { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime FechaPagoUtc { get; set; }
}

public class FacturaSaldoDTO
{
    public Guid FacturaGuid { get; set; }
    public decimal Total { get; set; }
    public decimal SaldoPendiente { get; set; }
    public string Moneda { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
