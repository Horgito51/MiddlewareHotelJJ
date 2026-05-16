using HotelJJ.API.Models.Requests.Reservas;

namespace HotelJJ.API.Models.Requests.Flujos;

public class IntegratedBookingCreateRequest
{
    public CreateReservationRequest Reserva { get; set; } = new();
    public bool GenerarFacturaInicial { get; set; } = true;
    public IntegratedPaymentRequest? PagoInicial { get; set; }
}

public class IntegratedPaymentRequest
{
    public Guid? FacturaGuid { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public bool EsPagoElectronico { get; set; }
    public string ProveedorPasarela { get; set; } = string.Empty;
    public string? TransaccionExterna { get; set; }
    public string? CodigoAutorizacion { get; set; }
    public string? Referencia { get; set; }
    public string Moneda { get; set; } = "USD";
    public decimal TipoCambio { get; set; } = 1;
    public bool SimularPago { get; set; }
    public string? TokenPago { get; set; }
}

public class IntegratedCheckOutRequest
{
    public string? Observaciones { get; set; }
    public bool RequiereMantenimiento { get; set; }
    public bool GenerarFacturaFinal { get; set; }
    public Guid? ReservaGuid { get; set; }
}
