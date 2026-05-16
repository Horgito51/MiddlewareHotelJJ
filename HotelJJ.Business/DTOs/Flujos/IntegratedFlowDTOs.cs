using HotelJJ.Business.DTOs.Facturacion;
using HotelJJ.Business.DTOs.Hospedaje;
using HotelJJ.Business.DTOs.Reservas;

namespace HotelJJ.Business.DTOs.Flujos;

public class IntegratedBookingCreateDTO
{
    public ReservationCreateDTO Reserva { get; set; } = new();
    public bool GenerarFacturaInicial { get; set; } = true;
    public IntegratedPaymentDTO? PagoInicial { get; set; }
}

public class IntegratedPaymentDTO
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

public class IntegratedBookingResultDTO
{
    public ReservationDTO Reserva { get; set; } = new();
    public FacturaDTO? FacturaInicial { get; set; }
    public PagoDTO? Pago { get; set; }
    public PagoSimuladoDTO? PagoSimulado { get; set; }
}

public class IntegratedCheckInResultDTO
{
    public ReservationDTO Reserva { get; set; } = new();
    public EstadiaDTO Estadia { get; set; } = new();
}

public class IntegratedCheckOutDTO
{
    public string? Observaciones { get; set; }
    public bool RequiereMantenimiento { get; set; }
    public bool GenerarFacturaFinal { get; set; }
    public Guid? ReservaGuid { get; set; }
}

public class IntegratedCheckOutResultDTO
{
    public EstadiaDTO Estadia { get; set; } = new();
    public FacturaDTO? FacturaFinal { get; set; }
}
