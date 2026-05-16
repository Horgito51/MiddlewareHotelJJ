namespace HotelJJ.API.Models.Requests.Facturacion;

public class PagoSimularRequest
{
    public Guid ReservaGuid { get; set; }
    public decimal Monto { get; set; }
    public string TokenPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class PagoCreateRequest
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

public class GatewayPagoCreateRequest
{
    public int IdFactura { get; set; }
    public int IdReserva { get; set; }
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

public class GatewayPagoSimularRequest
{
    public int IdReserva { get; set; }
    public decimal Monto { get; set; }
    public string TokenPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class PublicPagoSimularRequest
{
    public Guid ReservaGuid { get; set; }
    public decimal Monto { get; set; }
    public string TokenPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}

public class PagoEstadoRequest
{
    public string NuevoEstado { get; set; } = string.Empty;
}

public class AnularFacturaRequest
{
    public string Motivo { get; set; } = string.Empty;
}
