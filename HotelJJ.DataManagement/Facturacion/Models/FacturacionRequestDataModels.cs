namespace HotelJJ.DataManagement.Facturacion.Models;

public class PagoCreateDataRequest
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

public class PagoSimularDataRequest
{
    public int IdReserva { get; set; }
    public decimal Monto { get; set; }
    public string TokenPago { get; set; } = string.Empty;
    public string? Referencia { get; set; }
}
